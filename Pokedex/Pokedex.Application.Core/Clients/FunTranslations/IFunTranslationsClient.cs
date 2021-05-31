using Pokedex.Application.Core.Clients.FunTranslations.Models;
using Refit;
using System.Threading.Tasks;

namespace Pokedex.Application.Core.Clients.FunTranslations
{
    public interface IFunTranslationsClient
    {
        [Post("/translate/shakespeare")]
        public Task<IApiResponse<TranslationResponse>> ToShakespeareAsync([Body] TranslationRequest translationRequest);

        [Post("/translate/yoda")]
        public Task<IApiResponse<TranslationResponse>> ToYodaAsync([Body] TranslationRequest translationRequest);
    }
}
