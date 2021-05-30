using System.Text.Json.Serialization;

namespace Pokedex.Application.Core.Clients.PokeAPI.Models
{
    public class PokemonHabitat
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
