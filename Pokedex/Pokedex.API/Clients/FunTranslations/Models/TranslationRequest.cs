using System.Text.Json.Serialization;

namespace Pokedex.API.Clients.FunTranslations.Models
{
    public class TranslationRequest
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
