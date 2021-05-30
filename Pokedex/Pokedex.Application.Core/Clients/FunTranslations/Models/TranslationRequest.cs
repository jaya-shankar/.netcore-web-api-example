using System.Text.Json.Serialization;

namespace Pokedex.Application.Core.Clients.FunTranslations.Models
{
    public class TranslationRequest
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
