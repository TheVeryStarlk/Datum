using System.Text.Json.Nodes;
using Datum.Extractor.Extractors.Protocol;

namespace Datum.Extractor;

public sealed class BedrockDatum : Datum
{
    /// <summary>
    /// Gets the protocol.
    /// </summary>
    public BedrockProtocol? Protocol => protocol ??= Extract<BedrockProtocol>();

    private BedrockProtocol? protocol;

    internal BedrockDatum(string folder, IDictionary<string, JsonNode?> features) : base(folder, features)
    {
    }
}