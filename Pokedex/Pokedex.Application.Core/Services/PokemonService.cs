using Pokedex.Application.Core.Clients.FunTranslations;
using Pokedex.Application.Core.Clients.FunTranslations.Models;
using Pokedex.Application.Core.Clients.PokeAPI;
using Pokedex.Application.Core.Clients.PokeAPI.Models;
using Pokedex.Application.Core.Entities;
using Refit;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pokedex.Application.Core.Services
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

        public async Task<PokemonEntity> GetPokemonAsync(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                throw new ArgumentNullException(nameof(pokemonName));
            }

            IApiResponse<Pokemon> _Response = await __PokeAPIClient.GetSpeciesAsync(pokemonName.ToLower());

            if (_Response.StatusCode == HttpStatusCode.NotFound)
            {
                return new();
            }

            if (!_Response.IsSuccessStatusCode)
            {
                // Could also log here to monitor any external service failures + exceptions.
                throw new Exception($"PokeAPI Service Exception - {_Response.StatusCode}", _Response.Error);
            }

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
            PokemonEntity _Pokemon = await GetPokemonAsync(name.ToLower());

            if (_Pokemon.Exists && !string.IsNullOrEmpty(_Pokemon.Description))
            {
                TranslationRequest _Request = new()
                {
                    Text = _Pokemon.Description
                };

                IApiResponse<TranslationResponse> _Response = _Pokemon.Habitat.ToLower() == "cave" || _Pokemon.IsLegendary ? await __FunTranslationsClient.ToYodaAsync(_Request) : await __FunTranslationsClient.ToShakespeareAsync(_Request);

                if (_Response.IsSuccessStatusCode)
                {
                    _Pokemon.Description = _Response.Content.Content.TranslatedText;
                }
            }

            return _Pokemon;
        }
    }
}
