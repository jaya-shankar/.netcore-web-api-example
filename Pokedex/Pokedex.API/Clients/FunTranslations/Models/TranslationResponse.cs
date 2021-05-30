using System.Text.Json.Serialization;

namespace Pokedex.API.Clients.FunTranslations.Models
{
    public class TranslationResponse
    {
        [JsonPropertyName("contents")]
        public TranslatedContent Content { get; set; }
    }
}
