using System.Text.Json.Nodes;
using Datum.Extractor.Extractors;
using Version = Datum.Extractor.Extractors.Version;

namespace Datum.Extractor;

/// <summary>
/// Represents a Datum extractor.
/// </summary>
public sealed class Datum
{
    /// <summary>
    /// Gets the version.
    /// </summary>
    public Version? Version => version ??= Extract<Version>();

    private Version? version;

    /// <summary>
    /// Gets the protocol.
    /// </summary>
    public Protocol? Protocol => protocol ??= Extract<Protocol>();

    private Protocol? protocol;

    /// <summary>
    /// Gets the blocks information.
    /// </summary>
    public Block? Block => block ??= Extract<Block>();

    private Block? block;

    private readonly string folder;
    private readonly IDictionary<string, JsonNode?> features;

    internal Datum(string folder, IDictionary<string, JsonNode?> features)
    {
        this.folder = folder;
        this.features = features;
    }

    /// <summary>
    /// Extracts the specified type of extractor.
    /// </summary>
    /// <typeparam name="T">The type of the extractor.</typeparam>
    /// <returns>An instance of the specified extractor type.</returns>
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

        using var stream = File.OpenRead(Path.Join(folder, clean, $"{T.Name}.json"));

        var node = JsonNode.Parse(stream);

        return T.Create(node!);
    }
}