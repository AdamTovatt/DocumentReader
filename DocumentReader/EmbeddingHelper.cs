using CarelessApi.Models.Vectors;
using DocumentReader.Models;
using DocumentReader.Repositories;

namespace DocumentReader
{
    public class EmbeddingHelper
    {
        public static EmbeddingHelper Instance
        {
            get
            {
                if (_instance == null) _instance = new EmbeddingHelper();
                return _instance;
            }
        }

        private static EmbeddingHelper? _instance;
        private OpenAiApi openAi;

        private EmbeddingHelper()
        {
            openAi = new OpenAiApi(EnvironmentHelper.GetOpenAiApiKey());
        }

        public async Task<StoredEmbedding> GetEmbeddingAsync(string text, IStoredEmbeddingRepository storedEmbeddingRepository)
        {
            string contentHash = text.GetContentHash();
            StoredEmbedding? storedEmbedding = await storedEmbeddingRepository.GetByContentHashAsync(contentHash);

            if (storedEmbedding != null) // return the stored embedding we already have, if we have it
                return storedEmbedding;

            GetEmbeddingsResult getEmbeddingsResult = await openAi.GetEmbeddingsAsync(text);

            if (getEmbeddingsResult.Error != null)
                throw new Exception($"Error when getting embeddings for text: {getEmbeddingsResult.Error.Message}");

            EmbeddingData? data = getEmbeddingsResult.Data.FirstOrDefault();

            if (data == null)
                throw new Exception($"No embedding data was returned for text: {text}");

            StoredEmbedding result = new StoredEmbedding(text, contentHash, data.Embedding);

            await storedEmbeddingRepository.Write(result);

            return result;
        }
    }
}
