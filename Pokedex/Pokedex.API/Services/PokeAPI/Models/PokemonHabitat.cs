using System.Text.Json.Serialization;

namespace Pokedex.API.Services.PokeAPI.Models
{
    public class PokemonHabitat
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
