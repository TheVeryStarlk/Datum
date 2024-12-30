using System.Collections.Frozen;
using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors.Protocol;

public sealed class BedrockProtocol : IProtocol<BedrockProtocol>
{
    public static string Name => "protocol";

    public FrozenDictionary<int, Packet>? Packets => packets ??= Packet.Extract(node["types"]?.AsObject());

    private FrozenDictionary<int, Packet>? packets;

    private readonly JsonNode node;

    private BedrockProtocol(JsonNode node) => this.node = node;

    public static BedrockProtocol Create(JsonNode node) => new(node);
}