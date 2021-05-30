using System.Threading.Tasks;

namespace Pokedex.API.Services
{
    public interface IPokemonService
    {
        public Task<PokemonEntity> GetPokemonAsync(string name);
        public Task<PokemonEntity> GetTranslatedPokemonAsync(string name);
    }
}