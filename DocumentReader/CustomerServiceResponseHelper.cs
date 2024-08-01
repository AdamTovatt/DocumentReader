using DocumentReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentReader
{
    public class CustomerServiceResponseHelper
    {
        private const string prompt = @"Du är en assistent som svarar på kunders frågor med hjälp och endast med hjälp av din interna dokumentation
                                        som du får tillsammans med kundens fråga. Om frågan är oklar eller om det inte verkar som dokumentationen kan svara på den
                                        ska du inte svara. Kom ihåg att alltid svara så kort som möjligt och trevligt.";

        private OpenAiApi openAi;

        public CustomerServiceResponseHelper()
        {
            openAi = new OpenAiApi(EnvironmentHelper.GetOpenAiApiKey());
        }

        public async Task<string> GetResponseToQuestionAsync(string question, string relevantContext)
        {
            CompletionParameter completionParameter = new CompletionParameter("gpt-3.5-turbo");

            completionParameter.AddSystemMessage(prompt);
            completionParameter.AddUserMessage($"Här är dokumentation som kan vara relevant:\n\r***\n\r{relevantContext}\n\r***\n\rHär är kundens fråga:\n\r{question}");

            CompletionResult completionResult = await openAi.CompleteAsync(completionParameter);

            return completionResult.Choices.First().Message.Content;
        }
    }
}
