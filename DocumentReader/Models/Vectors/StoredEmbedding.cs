using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocumentReader.Models.Vectors
{
    public class StoredEmbedding : IVectorObject
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("textContent")]
        public string TextContent { get; set; }

        [JsonPropertyName("contentHash")]
        public string ContentHash { get; set; }

        [JsonPropertyName("coordinates")]
        public float[] Coordinates { get; set; }

        [JsonConstructor]
        public StoredEmbedding(int id, string textContent, string contentHash, float[] coordinates)
        {
            Id = id;
            TextContent = textContent;
            ContentHash = contentHash;
            Coordinates = coordinates;
        }

        public StoredEmbedding(string textContent, string contentHash, float[] coordinates)
        {
            TextContent = textContent;
            ContentHash = contentHash;
            Coordinates = coordinates;
        }

        public float[] GetVector()
        {
            return Coordinates;
        }

        public float? GetMinimumSimilarity()
        {
            return 0;
        }

        public string ToJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            return JsonSerializer.Serialize(this, options);
        }

        public static StoredEmbedding FromJson(string json)
        {
            StoredEmbedding? result = JsonSerializer.Deserialize<StoredEmbedding>(json);

            if (result == null)
                throw new JsonException("Could not deserialize the JSON string to a StoredEmbedding object");

            return result;
        }
    }
}
