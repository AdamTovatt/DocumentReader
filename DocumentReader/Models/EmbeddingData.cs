using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentReader.Models
{
    public class EmbeddingData
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("embedding")]
        public float[] Embedding { get; set; }

        public EmbeddingData(int index, float[] embedding)
        {
            Index = index;
            Embedding = embedding;
        }
    }
}
