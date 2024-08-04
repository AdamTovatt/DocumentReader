using DocumentReader.Models.Vectors;

namespace DocumentReader.Repositories
{
    public class FileSystemStoredEmbeddingRepository : IStoredEmbeddingRepository
    {
        private string directoryPath;

        public FileSystemStoredEmbeddingRepository(string directoryPath)
        {
            this.directoryPath = directoryPath;

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        public async Task<StoredEmbedding?> GetByContentHashAsync(string contentHash)
        {
            if(File.Exists(Path.Combine(directoryPath, $"{contentHash}.json")))
            {
                string fileContent = await File.ReadAllTextAsync(Path.Combine(directoryPath, $"{contentHash}.json"));
                return StoredEmbedding.FromJson(fileContent);
            }

            return null;
        }

        public async Task Write(StoredEmbedding storedEmbedding)
        {
            await File.WriteAllTextAsync(Path.Combine(directoryPath, $"{storedEmbedding.ContentHash}.json"), storedEmbedding.ToJson());
        }

        public async Task<List<StoredEmbedding>> GetAll()
        {
            List<StoredEmbedding> storedEmbeddings = new List<StoredEmbedding>();

            foreach(string file in Directory.EnumerateFiles(directoryPath))
            {
                string fileContent = await File.ReadAllTextAsync(file);
                storedEmbeddings.Add(StoredEmbedding.FromJson(fileContent));
            }

            return storedEmbeddings;
        }
    }
}
