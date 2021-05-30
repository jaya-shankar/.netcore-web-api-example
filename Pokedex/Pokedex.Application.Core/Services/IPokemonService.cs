using Pokedex.Application.Core.Entities;
using System.Threading.Tasks;

namespace Pokedex.Application.Core.Services
{
    public interface IPokemonService
    {
        public Task<PokemonEntity> GetPokemonAsync(string name);
        public Task<PokemonEntity> GetTranslatedPokemonAsync(string name);
    }
}
