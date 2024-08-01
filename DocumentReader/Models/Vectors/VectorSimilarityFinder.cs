namespace CarelessApi.Models.Vectors
{
    public class VectorSimilarityFinder
    {
        private float[] queryVector;
        private double queryMagnitude;

        public VectorSimilarityFinder(float[] queryVector)
        {
            this.queryVector = queryVector;
            SetQueryMagnitude();
        }

        public VectorSimilarityFinder(IVectorObject vectorObject)
        {
            queryVector = vectorObject.GetVector();
            SetQueryMagnitude();
        }

        private void SetQueryMagnitude()
        {
            for (int i = 0; i < queryVector.Length; i++)
                queryMagnitude += queryVector[i] * queryVector[i];

            queryMagnitude = Math.Sqrt(queryMagnitude);
        }

        /// <summary>
        /// Will return the cosine similarity between two vectors
        /// </summary>
        /// <param name="target">The target vector</param>
        /// <returns>The similarity between the target and the query vector, -1 representing not similar at all, 1 representing completely similar</returns>
        /// <exception cref="ArgumentException">Will be thrown if the vectors are not of the same length</exception>
        public float GetCosineSimilarity(float[] target)
        {
            if (target.Length != queryVector.Length)
                throw new ArgumentException("Vector lengths must be the same for cosine similarity calculation.");

            // Calculate the dot product of the two vectors
            float similarity = 0.0f;
            double targetMagnitude = 0.0;

            for (int i = 0; i < target.Length; i++)
            {
                similarity += target[i] * queryVector[i];
                targetMagnitude += target[i] * target[i];
            }

            targetMagnitude = Math.Sqrt(targetMagnitude);

            if (targetMagnitude == 0 || queryMagnitude == 0)
                return 0.0f; // Avoid division by zero

            // Calculate the cosine similarity
            return similarity / (float)(targetMagnitude * queryMagnitude);
        }
    }
}
