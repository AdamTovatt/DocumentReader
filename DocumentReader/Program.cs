using DocumentReader.Models.Vectors;
using DocumentReader.Repositories;

namespace DocumentReader
{
    internal class Program
    {
        private const string documentsDirectory = "documents";

        static async Task Main(string[] args)
        {
            if(!Directory.Exists(documentsDirectory))
                Directory.CreateDirectory(documentsDirectory);

            FileSystemStoredEmbeddingRepository embeddedDocumentsRepository = new FileSystemStoredEmbeddingRepository("documents_embedded");
            FileSystemStoredEmbeddingRepository userInputRepository = new FileSystemStoredEmbeddingRepository("user_input_embedded");

            VectorCollection<StoredEmbedding> embeddedDocuments = new VectorCollection<StoredEmbedding>();

            foreach(string file in Directory.EnumerateFiles(documentsDirectory))
            {
                string fileContent = await FileHelper.GetFileContentAsync(file);
                StoredEmbedding storedEmbedding = await EmbeddingHelper.Instance.GetEmbeddingAsync(fileContent, embeddedDocumentsRepository);
                embeddedDocuments.Add(storedEmbedding);
            }

            CustomerServiceResponseHelper customerServiceAi = new CustomerServiceResponseHelper();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Ställ din fråga till Retendo: ");
                string? userInput = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(userInput))
                    break;

                StoredEmbedding userInputEmbedding = await EmbeddingHelper.Instance.GetEmbeddingAsync(userInput, userInputRepository);

                StoredEmbedding queryResult = embeddedDocuments.FindNearest(userInputEmbedding.Coordinates, 1).First();

                Console.WriteLine("Sökning i intern dokumentation klar.");
                Console.WriteLine("Skriver ett svar...");

                string aiResponse = await customerServiceAi.GetResponseToQuestionAsync(userInput, queryResult.TextContent);

                Console.Clear();

                Console.WriteLine("Din fråga:");
                Console.WriteLine(userInput);
                Console.WriteLine();

                Console.WriteLine("Svar:");
                Console.WriteLine(aiResponse);
                Console.WriteLine();
                Console.WriteLine("Baserat på dokumentet:");
                Console.WriteLine(queryResult.TextContent);

                Console.ReadLine();
            }
        }
    }
}
