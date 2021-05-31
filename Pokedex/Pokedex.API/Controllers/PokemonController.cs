using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokedex.API.Models;
using Pokedex.Application.Core.Entities;
using Pokedex.Application.Core.Services;
using System.Threading.Tasks;

namespace Pokedex.API.Controllers
{
    [ApiController]
    [Route("pokemon")]
    public class PokemonController : ControllerBase
    {
        private readonly IMapper __Mapper;
        private readonly IPokemonService __PokemonService;

        public PokemonController(IPokemonService pokemonService, IMapper mapper)
        {
            __PokemonService = pokemonService;
            __Mapper = mapper;
        }

        [HttpGet("{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string name)
        {
            PokemonEntity _Entity = await __PokemonService.GetPokemonAsync(name);

            if (_Entity.Exists)
            {
                return Ok(__Mapper.Map<PokemonModel>(_Entity));
            }

            return NotFound();
        }

        [HttpGet("translated/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTranslatedPokemon(string name)
        {
            PokemonEntity _Entity = await __PokemonService.GetTranslatedPokemonAsync(name);

            if (_Entity.Exists)
            {
                return Ok(__Mapper.Map<PokemonModel>(_Entity));
            }

            return NotFound();
        }
    }
}
