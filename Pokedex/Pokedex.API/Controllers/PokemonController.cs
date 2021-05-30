using Microsoft.AspNetCore.Mvc;
using Pokedex.API.Services;
using System.Threading.Tasks;

namespace Pokedex.API.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService __PokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            __PokemonService = pokemonService;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            PokemonEntity _Entity = await __PokemonService.GetPokemonAsync(name);

            if (_Entity.Exists)
            {
                return Ok(_Entity);
            }

            return NotFound();
        }

        [HttpGet("translate/{name}")]
        public async Task<IActionResult> GetTranslatedPokemon(string name)
        {
            PokemonEntity _Entity = await __PokemonService.GetTranslatedPokemonAsync(name);

            if (_Entity.Exists)
            {
                return Ok(_Entity);
            }

            return NotFound();
        }
    }
}
