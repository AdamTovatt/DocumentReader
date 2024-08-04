namespace DocumentReader.Models.Vectors
{
    public class EmbeddedObject<T> : IVectorObject
    {
        public T Value { get; set; }
        public string Content { get; set; }
        private float[] coordinates;
        private float? minimumSimilarity;

        public EmbeddedObject(T value, float? minimumSimilarity, float[] coordinates, string content)
        {
            Value = value;
            this.coordinates = coordinates;
            this.minimumSimilarity = minimumSimilarity;
            Content = content;
        }

        public float? GetMinimumSimilarity()
        {
            return minimumSimilarity;
        }

        public float[] GetVector()
        {
            return coordinates;
        }
    }
}
