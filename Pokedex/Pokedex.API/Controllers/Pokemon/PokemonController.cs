using Microsoft.AspNetCore.Mvc;

namespace Pokedex.API.Controllers.Pokemon
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            return Ok(name);
        }

        [HttpGet("translate/{name}")]
        public IActionResult GetTranslatedPokemon(string name)
        {
            return Ok(name);
        }
    }
}
