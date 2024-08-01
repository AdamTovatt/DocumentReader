using NUglify;

namespace DocumentReader
{
    public class FileHelper
    {
        public static async Task<string> GetFileContentAsync(string path)
        {
            string fileName = Path.GetFileName(path);
            string content = await GetFileContentInternalAsync(path);

            return $"{fileName}\n\r{content}";
        }

        private static async Task<string> GetFileContentInternalAsync(string path)
        {
            string fileExtension = Path.GetExtension(path);
            string rawContent = await File.ReadAllTextAsync(path);

            if (fileExtension == ".html")
                return ExtractTextFromHtml(rawContent);

            return rawContent;
        }

        private static string ExtractTextFromHtml(string html)
        {
            return Uglify.HtmlToText(html).Code.Replace("\u00A0", " ");
        }
    }
}
