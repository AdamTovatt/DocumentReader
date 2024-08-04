using DocumentReader.Models.Vectors;

namespace DocumentReader.Repositories
{
    public interface IStoredEmbeddingRepository
    {
        public Task<StoredEmbedding?> GetByContentHashAsync(string contentHash);
        public Task Write(StoredEmbedding storedEmbedding);
        public Task<List<StoredEmbedding>> GetAll();
    }
}
