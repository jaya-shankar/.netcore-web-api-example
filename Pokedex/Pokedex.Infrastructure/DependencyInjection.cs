using Microsoft.Extensions.DependencyInjection;
using Pokedex.Application.Core.Clients.FunTranslations;
using Pokedex.Application.Core.Clients.PokeAPI;
using Refit;
using System;

namespace Pokedex.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services
                .AddRefitClient<IPokeAPIClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://pokeapi.co/api/v2"));

            services
                .AddRefitClient<IFunTranslationsClient>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.funtranslations.com"));
        }
    }
}
