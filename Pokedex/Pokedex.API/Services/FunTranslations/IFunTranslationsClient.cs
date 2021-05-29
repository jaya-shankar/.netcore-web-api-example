using Pokedex.API.Services.FunTranslations.Models;
using Refit;
using System.Threading.Tasks;

namespace Pokedex.API.Services.FunTranslations
{
    public interface IFunTranslationsClient
    {
        [Post("/translate/shakespeare")]
        public Task<ApiResponse<TranslationResponse>> ToShakespeareAsync([Body] TranslationRequest translationRequest);

        [Post("/translate/yoda")]
        public Task<ApiResponse<TranslationResponse>> ToYodaAsync([Body] TranslationRequest translationRequest);
    }
}
