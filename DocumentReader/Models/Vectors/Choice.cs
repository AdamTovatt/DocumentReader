using DocumentReader;
using DocumentReader.Repositories;

namespace CarelessApi.Models.Vectors
{
    public class Choice<T>
    {
        public T Value { get; set; }
        public float? MinimumSimilarity { get; set; }

        private List<ChoiceExample> choiceExamples = new List<ChoiceExample>();

        public Choice(T value, float? minimumSimilarity = null)
        {
            Value = value;
            MinimumSimilarity = minimumSimilarity;
        }

        public async Task<List<EmbeddedObject<T>>> GetEmbeddedObjects(IStoredEmbeddingRepository storedEmbeddingRepository)
        {
            List<EmbeddedObject<T>> embeddedObjects = new List<EmbeddedObject<T>>();

            foreach (ChoiceExample example in choiceExamples)
            {
                float[] embeddings = (await EmbeddingHelper.Instance.GetEmbeddingAsync(example.Input, storedEmbeddingRepository)).Coordinates;

                float? minimumSimilarityToUse = MinimumSimilarity;
                if (minimumSimilarityToUse == null)
                    minimumSimilarityToUse = example.MinimumSimilarity;

                embeddedObjects.Add(new EmbeddedObject<T>(Value, minimumSimilarityToUse, embeddings, example.Input));
            }

            return embeddedObjects.ToList();
        }

        public Choice<T> AddExample(string input, float? minimumSimilarity = null)
        {
            choiceExamples.Add(new ChoiceExample(input, minimumSimilarity));
            return this; // for nice method chaining syntax
        }

        private class ChoiceExample
        {
            public string Input { get; set; }
            public float? MinimumSimilarity { get; set; }

            public ChoiceExample(string input, float? minimumSimilarity = null)
            {
                Input = input;
                MinimumSimilarity = minimumSimilarity;
            }
        }
    }
}
