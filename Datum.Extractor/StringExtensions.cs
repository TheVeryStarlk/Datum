namespace Datum.Extractor;

internal static class StringExtensions
{
    public static string FixPathSeparator(this string source) => source.Replace('/', Path.DirectorySeparatorChar);
}