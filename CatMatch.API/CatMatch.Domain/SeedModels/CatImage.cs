using System.Text.Json.Serialization;

namespace CatMatch.Domain.SeedModels
{
    public class CatImage
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("id")]
        public string OriginalId { get; set; }
    }
}
