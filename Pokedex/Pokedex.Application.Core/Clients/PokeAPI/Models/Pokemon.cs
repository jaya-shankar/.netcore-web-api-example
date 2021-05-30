using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pokedex.Application.Core.Clients.PokeAPI.Models
{
    public class Pokemon
    {
        [JsonPropertyName("flavor_text_entries")]
        public List<FlavorText> Descriptions { get; set; }

        [JsonPropertyName("habitat")]
        public PokemonHabitat Habitat { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
