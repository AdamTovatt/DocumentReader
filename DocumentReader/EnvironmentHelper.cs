using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentReader
{
    public static class EnvironmentHelper
    {
        public static string GetOpenAiApiKey()
        {
            if (!File.Exists("openai-api-key.txt"))
                throw new Exception("Could not find openai-api-key.txt file in the current directory. Please create a file with your OpenAI API key and try again. (https://platform.openai.com/docs/guides/authentication#api-keys");

            return File.ReadAllText("openai-api-key.txt").Trim();
        }
    }
}
