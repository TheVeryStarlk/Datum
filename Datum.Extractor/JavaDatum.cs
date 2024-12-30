using System.Text.Json.Nodes;
using Datum.Extractor.Extractors.Protocol;

namespace Datum.Extractor;

/// <summary>
/// Represents a Minecraft Datum extractor.
/// </summary>
public sealed class JavaDatum : Datum
{
    /// <summary>
    /// Gets the protocol.
    /// </summary>
    public JavaProtocol? Protocol => protocol ??= Extract<JavaProtocol>();

    private JavaProtocol? protocol;

    internal JavaDatum(string folder, IDictionary<string, JsonNode?> features) : base(folder, features)
    {
    }
}