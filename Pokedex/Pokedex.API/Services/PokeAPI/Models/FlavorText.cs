using System.Text.Json.Serialization;

namespace Pokedex.API.Services.PokeAPI.Models
{
    public class FlavorText
    {
        [JsonPropertyName("language")]
        public Language Language { get; set; }

        [JsonPropertyName("flavor_text")]
        public string Value { get; set; }
    }
}
