using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors;

public sealed class Protocol(JsonNode node) : IExtractor<Protocol>
{
    public static string Name => "protocol";

    public static Protocol Create(JsonNode node)
    {
        return new Protocol(node);
    }
}