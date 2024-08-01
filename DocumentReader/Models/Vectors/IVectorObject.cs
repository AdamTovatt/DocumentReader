namespace CarelessApi.Models.Vectors
{
    /// <summary>
    /// A vector data object
    /// </summary>
    public interface IVectorObject
    {
        public float[] GetVector();
        public float? GetMinimumSimilarity();
    }
}
