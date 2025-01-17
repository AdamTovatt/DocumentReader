﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocumentReader.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("param")]
        public string? Param { get; set; }
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        public ErrorResponse(string message, string type, string? param, string? code)
        {
            Message = message;
            Type = type;
            Param = param;
            Code = code;
        }
    }
}
