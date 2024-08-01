using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace DocumentReader.Models
{
    public class GetEmbeddingsResult : OpenAiApiResponse
    {
        [JsonPropertyName("data")]
        public EmbeddingData[] Data { get; set; }
        public string? InputText { get; set; }

        public GetEmbeddingsResult(EmbeddingData[] data)
        {
            Data = data;
        }

        public static GetEmbeddingsResult FromJson(string json)
        {
            GetEmbeddingsResult? result = JsonSerializer.Deserialize<GetEmbeddingsResult>(json);

            if (result == null) throw new JsonException($"Could not deserialize typeof @{typeof(GetEmbeddingsResult)} from json with lengt {json.Length}: {json}");
            return result;
        }

        public GetEmbeddingsResult SetInputText(string inputText)
        {
            InputText = inputText;
            return this;
        }
    }
}
