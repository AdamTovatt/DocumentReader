using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentReader.Models
{
    public class Choice
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }

        public Choice(int index, Message message, string finishReason)
        {
            Index = index;
            Message = message;
            FinishReason = finishReason;
        }

        public override string ToString()
        {
            return Message.ToString();
        }
    }
}
