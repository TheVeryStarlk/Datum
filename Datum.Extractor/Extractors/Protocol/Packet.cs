using System.Collections.Frozen;
using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors.Protocol;

/// <summary>
/// Represents a packet with a name and properties.
/// </summary>
/// <param name="Name">Gets the packet's name.</param>
/// <param name="Properties">Gets the packet's properties.</param>
public sealed record Packet(string Name, Property[] Properties)
{
    internal static FrozenDictionary<int, Packet>? Java(JsonObject? types)
    {
        var packets = new Dictionary<int, Packet>();

        if (types is null)
        {
            return null;
        }

        for (var identifier = 0; identifier < types.Count - 1; identifier++)
        {
            var packet = types[identifier];
            var name = packet!.GetPath().Split('.').Last();

            if (packet[1]! is not JsonArray items)
            {
                continue;
            }

            var properties = new Property[items.Count];

            for (var index = 0; index < items.Count; index++)
            {
                var item = items[index];
                properties[index] = new Property(item![0]!.ToString(), item[1]!.ToString());
            }

            packets[identifier] = new Packet(name, properties);
        }

        return packets.ToFrozenDictionary();
    }

    internal static FrozenDictionary<int, Packet>? Bedrock(JsonObject? types) => null;
}

/// <summary>
/// Represents a property with a name and type.
/// </summary>
/// <param name="Name">Gets the property's name.</param>
/// <param name="Type">Gets the property's value type.</param>
public sealed record Property(string Name, string Type);