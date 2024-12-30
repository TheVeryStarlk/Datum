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
    internal static FrozenDictionary<int, Packet>? Extract(JsonObject? types)
    {
        if (types is null)
        {
            return null;
        }

        var packets = new Dictionary<int, Packet>();

        for (var identifier = types.Count - 1; identifier >= 0; identifier--)
        {
            var packet = types[identifier];

            // We reached the types section, no more packets, exit.
            if (packet!.ToString() is "native")
            {
                break;
            }

            var name = packet.GetPath().Split('.').Last();

            if (name is "packet" || packet[1]! is not JsonArray items)
            {
                continue;
            }

            var properties = new Property[items.Count];

            for (var index = 0; index < items.Count; index++)
            {
                var item = items[index];
                var type = item![1] is JsonValue ? item[1]!.ToString() : item[0]!.ToString();

                properties[index] = new Property(item[0]!.ToString(), type);
            }

            packets[identifier] = new Packet(name, properties);
        }

        return packets.Count is 0 ? null : packets.ToFrozenDictionary();
    }
}

/// <summary>
/// Represents a property with a name and type.
/// </summary>
/// <param name="Name">Gets the property's name.</param>
/// <param name="Type">Gets the property's value type.</param>
public sealed record Property(string Name, string Type);