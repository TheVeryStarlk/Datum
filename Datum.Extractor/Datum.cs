using System.Text.Json.Nodes;
using Datum.Extractor.Extractors;

namespace Datum.Extractor;

public sealed class Datum(IDictionary<string, JsonNode?> features)
{
    public async Task<T?> ExtractAsync<T>(CancellationToken cancellationToken) where T : IExtractor<T>
    {
        if (!features.TryGetValue(T.Name, out var feature))
        {
            return default;
        }

        var clean = feature!
            .ToString()
            .Replace("\"", string.Empty)
            .FixPathSeparator();

        await using var stream = File.OpenRead(Path.Join(DatumExtractor.Folder, clean, $"{T.Name}.json"));

        var node = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken);

        return T.Create(node!);
    }
}