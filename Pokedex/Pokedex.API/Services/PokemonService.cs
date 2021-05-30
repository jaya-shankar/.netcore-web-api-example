using Pokedex.API.Clients.FunTranslations;
using Pokedex.API.Clients.FunTranslations.Models;
using Pokedex.API.Clients.PokeAPI;
using Pokedex.API.Clients.PokeAPI.Models;
using Refit;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pokedex.API.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IFunTranslationsClient __FunTranslationsClient;
        private readonly IPokeAPIClient __PokeAPIClient;

        public PokemonService(IPokeAPIClient pokeAPIClient, IFunTranslationsClient funTranslationsClient)
        {
            __PokeAPIClient = pokeAPIClient;
            __FunTranslationsClient = funTranslationsClient;
        }

        public async Task<PokemonEntity> GetPokemonAsync(string name)
        {
            ApiResponse<Pokemon> _Response = await __PokeAPIClient.GetSpeciesAsync(name);

            if (_Response.StatusCode == HttpStatusCode.NotFound)
            {
                return new();
            }

            await _Response.EnsureSuccessStatusCodeAsync();

            return new()
            {
                Exists = true,
                Description = _Response.Content.Descriptions?.Where(d => d.Language.Name == "en").FirstOrDefault()?.Value ?? string.Empty,
                Name = _Response.Content.Name,
                Habitat = _Response.Content.Habitat.Name,
                IsLegendary = _Response.Content.IsLegendary
            };
        }

        public async Task<PokemonEntity> GetTranslatedPokemonAsync(string name)
        {
            PokemonEntity _Pokemon = await GetPokemonAsync(name);

            if (_Pokemon.Exists)
            {
                TranslationRequest _Request = new()
                {
                    Text = _Pokemon.Description
                };

                ApiResponse<TranslationResponse> _Response = _Pokemon.Habitat.ToLower() == "cave" || _Pokemon.IsLegendary ? await __FunTranslationsClient.ToYodaAsync(_Request) : await __FunTranslationsClient.ToShakespeareAsync(_Request);

                if (_Response.IsSuccessStatusCode)
                {
                    _Pokemon.Description = _Response.Content.Content.TranslatedText;
                }
            }

            return _Pokemon;
        }
    }
}
