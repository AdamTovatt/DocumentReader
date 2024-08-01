using System.Text.Json;

namespace CarelessApi.Models.Vectors
{
    public class VectorCollection<T>
        where T : IVectorObject
    {
        private List<T> vectors = new List<T>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorCollection{T}"/> class.
        /// Will create a new vector collection made to contain vectors
        /// </summary>
        public VectorCollection() { }

        /// <summary>
        /// Will add a vector to the collection
        /// </summary>
        /// <param name="vector">The vector to add</param>
        public void Add(T vector)
        {
            vectors.Add(vector);
        }

        /// <summary>
        /// Will add multiple vectors to the collection
        /// </summary>
        /// <param name="vectors">The vectors to add</param>
        public void AddRange(IEnumerable<T> vectors)
        {
            this.vectors.AddRange(vectors);
        }

        /// <summary>
        /// Will get a value from the collections
        /// </summary>
        /// <param name="index">The index to get the value at</param>
        /// <returns>The vector stored at the provided index</returns>
        public T GetValue(int index)
        {
            return vectors[index];
        }

        /// <summary>
        /// Will set a value at an index
        /// </summary>
        /// <param name="index">The index to set a value at</param>
        /// <param name="value">The value to set</param>
        public void SetValue(int index, T value)
        {
            vectors[index] = value;
        }

        /// <summary>
        /// Index operator for this collection, can be used to get and set values
        /// </summary>
        /// <param name="key">The index to get or set a value at</param>
        /// <returns></returns>
        public T this[int key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        /// <summary>
        /// Find nearest vector by comparing all vectors to the query
        /// </summary>
        /// <param name="query">The vector to search for</param>
        /// <param name="count">The amount of closest vectors to find</param>
        /// <returns>A list of the closest vectors</returns>
        public List<T> FindNearest(float[] query, int count)
        {
            VectorSimilarityFinder similarityFinder = new VectorSimilarityFinder(query);

            List<(T, float)> result = new List<(T, float)>();

            float worstIncludedSimilarity = -1f;

            for (int i = 0; i < vectors.Count; i++)
            {
                T vector = vectors[i];
                float similarity = similarityFinder.GetCosineSimilarity(vector.GetVector());
                float? minimumSimilarity = vector.GetMinimumSimilarity();

                if ((minimumSimilarity == null || similarity > minimumSimilarity) && (similarity > worstIncludedSimilarity || result.Count < count))
                {
                    if (result.Count >= count)
                    {
                        float lowestValue = 1f;
                        int lowestIndex = 0;

                        for (int j = 0; j < result.Count; j++)
                        {
                            if (result[j].Item2 < lowestValue)
                            {
                                lowestValue = result[j].Item2;
                                lowestIndex = j;
                            }
                        }

                        result.RemoveAt(lowestIndex);
                    }

                    result.Add((vector, similarity));
                    worstIncludedSimilarity = similarity;
                }
            }

            List<T> finalResult = new List<T>();

            result = result.OrderByDescending(x => x.Item2).ToList();

            for (int i = 0; i < Math.Min(count, result.Count); i++)
                finalResult.Add(result[i].Item1);

            return finalResult;
        }

        /// <summary>
        /// Will return a vector collection from a json string
        /// </summary>
        /// <param name="json">The json string</param>
        /// <returns>A possibly null VectorCollection</returns>
        public static VectorCollection<T>? FromJson(string json)
        {
            return JsonSerializer.Deserialize<VectorCollection<T>>(json);
        }

        /// <summary>
        /// Will return a json string from this vector collection
        /// </summary>
        /// <returns>A json string</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
