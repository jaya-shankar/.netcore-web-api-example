using System.Text.Json.Serialization;

namespace Pokedex.Application.Core.Clients.FunTranslations.Models
{
    public class TranslationResponse
    {
        [JsonPropertyName("contents")]
        public TranslatedContent Content { get; set; }
    }
}
