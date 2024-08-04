namespace DocumentReader.Models.Vectors
{
    public class TextChunkVector : IVectorObject
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ChunkIndex { get; set; }
        public int TextContentId { get; set; }
        public float[] Coordinates { get; set; }

        public TextChunkVector(int id, string content, int chunkIndex, int textContentId, float[] coordinates)
        {
            Id = id;
            Content = content;
            ChunkIndex = chunkIndex;
            TextContentId = textContentId;
            Coordinates = coordinates;
        }

        public float[] GetVector()
        {
            return Coordinates;
        }

        public override string ToString()
        {
            return Content;
        }

        public float? GetMinimumSimilarity()
        {
            return null;
        }
    }
}
