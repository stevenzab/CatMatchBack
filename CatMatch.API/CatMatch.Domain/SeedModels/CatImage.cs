using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
