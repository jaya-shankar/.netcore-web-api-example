using Microsoft.Extensions.DependencyInjection;
using Pokedex.Application.Core.Services;

namespace Pokedex.Application.Core
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPokemonService, PokemonService>();
        }
    }
}
