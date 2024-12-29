using System.Text.Json.Nodes;
using Datum.Extractor.Extractors;

namespace Datum.Extractor;

public sealed class Datum(IDictionary<string, JsonNode?> features)
{
    public Protocol? Protocol => protocol ??= Extract<Protocol>();

    private Protocol? protocol;

    private T? Extract<T>() where T : IExtractor<T>
    {
        if (!features.TryGetValue(T.Name, out var feature))
        {
            return default;
        }

        var clean = feature!
            .ToString()
            .Replace("\"", string.Empty)
            .FixPathSeparator();

        using var stream = File.OpenRead(Path.Join(DatumExtractor.Folder, clean, $"{T.Name}.json"));

        var node = JsonNode.Parse(stream);

        return T.Create(node!);
    }
}