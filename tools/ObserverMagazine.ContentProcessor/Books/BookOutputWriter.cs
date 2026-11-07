using System.Text.Json;

namespace ObserverMagazine.ContentProcessor.Books;

public static class BookOutputWriter
{
    public const string DataDirectoryName = "book-data";
    public const string IndexFileName = "books-index.json";
    public const string TocFileName = "toc.json";

    public static string Write(IReadOnlyList<CompiledBook> books, string outputDir, JsonSerializerOptions jsonOptions)
    {
        var root = Path.Combine(outputDir, DataDirectoryName);
        if (Directory.Exists(root))
        {
            Directory.Delete(root, recursive: true);
        }
        Directory.CreateDirectory(root);

        WriteJson(Path.Combine(root, IndexFileName), books.Select(b => b.Index).ToArray(), jsonOptions);

        foreach (var book in books)
        {
            var bookDir = Path.Combine(root, book.Toc.Slug);
            Directory.CreateDirectory(bookDir);
            WriteJson(Path.Combine(bookDir, TocFileName), book.Toc, jsonOptions);

            foreach (var document in book.Documents)
            {
                var chapterDir = Path.Combine(bookDir, document.ChapterSlug);
                Directory.CreateDirectory(chapterDir);
                WriteJson(Path.Combine(chapterDir, $"{document.Slug}.json"), document, jsonOptions);
            }
        }

        return root;
    }

    private static void WriteJson<T>(string path, T value, JsonSerializerOptions jsonOptions) =>
        File.WriteAllText(path, JsonSerializer.Serialize(value, jsonOptions));
}
