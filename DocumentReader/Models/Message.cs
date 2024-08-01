using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentReader.Models
{
    /// <summary>
    /// Represents both a message to and from OpenAi
    /// </summary>
    public class Message
    {
        /// <summary>
        /// The role that the message is from, if the message is from the system it will be "system", "user" if it's from the user and "assistant" if it's from the assistant
        /// </summary>
        [JsonPropertyName("role")]
        public string Role { get; set; }

        /// <summary>
        /// The text content of this message
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// Not really sure what this is for to be honest, maybe it should be removed?
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Constructor that is used when deserializing the object
        /// </summary>
        /// <param name="role"></param>
        /// <param name="content"></param>
        /// <param name="name"></param>
        /// <param name="functionCall"></param>
        [JsonConstructor]
        public Message(string role, string content, string? name)
        {
            Role = role;
            Content = content;
            Name = name;
        }

        /// <summary>
        /// Will create a message with the provided role, content and id
        /// </summary>
        /// <param name="role">The role of the sender. For example, if it is from the user the role should be "user"</param>
        /// <param name="content">The content of the message, maybe something like "Hello, I want help with a thing"</param>
        /// <param name="id">The id to use</param>
        public Message(string role, string content)
        {
            Role = role;
            Content = content;
        }

        /// <summary>
        /// Converts the message object to a string that is readable
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Role}: {Content}";
        }
    }
}
