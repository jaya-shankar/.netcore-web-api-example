using System.Text.Json.Serialization;

namespace Pokedex.API.Services.FunTranslations.Models
{
    public class TranslationResponse
    {
        [JsonPropertyName("contents")]
        public TranslatedContent Content { get; set; }
    }
}
