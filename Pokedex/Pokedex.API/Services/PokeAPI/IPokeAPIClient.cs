using Pokedex.API.Services.PokeAPI.Models;
using Refit;
using System.Threading.Tasks;

namespace Pokedex.API.Services.PokeAPI
{
    public interface IPokeAPIClient
    {
        [Get("/pokemon-species/{name}")]
        public Task<ApiResponse<Pokemon>> GetSpeciesAsync(string name);
    }
}
