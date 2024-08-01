using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentReader.Models
{
    public abstract class OpenAiApiResponse
    {
        [JsonPropertyName("object")]
        public string? Object { get; set; }
        [JsonPropertyName("error")]
        public ErrorResponse? Error { get; set; }
    }
}
