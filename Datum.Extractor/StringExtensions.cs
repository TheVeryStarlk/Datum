namespace Datum.Extractor;

internal static class StringExtensions
{
    public static string FixPathSeparator(this string source)
    {
        return source.Replace('/', Path.DirectorySeparatorChar);
    }
}