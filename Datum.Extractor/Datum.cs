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

    private readonly IDictionary<string, JsonNode?> features;

    /// <summary>
    /// Initializes a new instance of the <see cref="Datum"/> class.
    /// </summary>
    /// <param name="features">The dictionary of features.</param>
    public Datum(IDictionary<string, JsonNode?> features)
    {
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

        using var stream = File.OpenRead(Path.Join(DatumExtractor.Folder, clean, $"{T.Name}.json"));

        var node = JsonNode.Parse(stream);

        return T.Create(node!);
    }
}