using AutoMapper;
using Pokedex.API.Models;
using Pokedex.Application.Core.Entities;

namespace Pokedex.API
{
    internal class MappingProfile : Profile
    {
        internal MappingProfile()
        {
            CreateMap<PokemonEntity, PokemonModel>();
        }
    }
}
