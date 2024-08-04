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
        private const string prompt2 = @"Du är en assistent som svarar på kunders frågor med hjälp och endast med hjälp av din interna dokumentation
                                        som du får tillsammans med kundens fråga. Om frågan är oklar eller om det inte verkar som dokumentationen kan svara på den
                                        ska du inte svara. Kom ihåg att alltid svara så kort som möjligt och trevligt.";

        private const string prompt = @"Mål: Du är en kundtjänstchattbot som är utformad för att korrekt besvara frågor baserat på den tillhandahållna dokumentationen. Ditt mål är att ge hjälpsamma och korrekta svar på kundförfrågningar.

            Instruktioner:
            - Studera noggrant den tillhandahållna dokumentationen för att förstå vilken information som finns tillgänglig.
            - När en kund ställer en fråga, sök i dokumentationen efter relevant information för att formulera ett svar.
            - Om du hittar ett klart och fullständigt svar i dokumentationen, ge det svaret ordagrant utan att modifiera eller lägga till något.
            - Skriv aldrig ut några namn, personnummer eller annan identifierande information om sådan finns i dokumentationen.
            - Om dokumentationen inte innehåller ett fullständigt svar, meddela vänligt kunden att du inte har tillräcklig information för att fullt ut besvara deras fråga.
            - Hitta aldrig på information eller spekulera bortom vad som anges i dokumentationen.
            - Bibehåll en professionell och vänlig ton i dina svar.
            - Om en kunds fråga är oklar eller för bred, be vänligen om förtydligande innan du försöker svara.
            
            Din kunskap kommer enbart från den tillhandahållna dokumentationen. Du har ingen ytterligare information utöver vad som finns i de dokumenten. Svara sanningsenligt baserat på dokumentationen och försök aldrig att hitta på eller gissa svar.";

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
