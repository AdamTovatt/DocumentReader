using CarelessApi.Models.Vectors;
using DocumentReader.Repositories;

namespace DocumentReader
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if(!Directory.Exists("documents"))
                Directory.CreateDirectory("documents");

            FileSystemStoredEmbeddingRepository embeddedDocumentsRepository = new FileSystemStoredEmbeddingRepository("documents_embedded");
            FileSystemStoredEmbeddingRepository userInputRepository = new FileSystemStoredEmbeddingRepository("user_input_embedded");

            VectorCollection<StoredEmbedding> embeddedDocuments = new VectorCollection<StoredEmbedding>();

            foreach(string file in Directory.EnumerateFiles("documents"))
            {
                string fileContent = await FileHelper.GetFileContentAsync(file);
                StoredEmbedding storedEmbedding = await EmbeddingHelper.Instance.GetEmbeddingAsync(fileContent, embeddedDocumentsRepository);
                embeddedDocuments.Add(storedEmbedding);
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter some text to compare to the documents:");
                string? userInput = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(userInput))
                    break;

                StoredEmbedding userInputEmbedding = await EmbeddingHelper.Instance.GetEmbeddingAsync(userInput, userInputRepository);

                StoredEmbedding queryResult = embeddedDocuments.FindNearest(userInputEmbedding.Coordinates, 1).First();

                Console.WriteLine();
                Console.WriteLine(queryResult.TextContent);
                Console.ReadLine();
            }
        }
    }
}
