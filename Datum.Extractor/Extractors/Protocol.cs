using System.Collections.Frozen;
using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors;

public sealed class Protocol(JsonNode node) : IExtractor<Protocol>
{
    public sealed record Packet(string Name, Property[] Properties)
    {
        public static FrozenDictionary<int, Packet> Deserialize(JsonObject? types)
        {
            var packets = new Dictionary<int, Packet>();

            if (types is null)
            {
                return packets.ToFrozenDictionary();
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
    }

    public sealed record Property(string Name, string Type);

    public sealed class ServerMetadata(JsonObject node)
    {
        public FrozenDictionary<int, Packet> Handshake => handshake ??= Packet.Deserialize(node["handshaking"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? handshake;

        public FrozenDictionary<int, Packet> Status => status ??= Packet.Deserialize(node["status"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? status;

        public FrozenDictionary<int, Packet> Login => login ??= Packet.Deserialize(node["login"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? login;

        public FrozenDictionary<int, Packet> Configuration => configuration ??= Packet.Deserialize(node["configuration"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? configuration;

        public FrozenDictionary<int, Packet> Play => play ??= Packet.Deserialize(node["play"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? play;
    }

    public sealed class ClientMetadata(JsonObject node)
    {
        public FrozenDictionary<int, Packet> Handshake => handshake ??= Packet.Deserialize(node["handshaking"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? handshake;

        public FrozenDictionary<int, Packet> Status => status ??= Packet.Deserialize(node["status"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? status;

        public FrozenDictionary<int, Packet> Login => login ??= Packet.Deserialize(node["login"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? login;

        public FrozenDictionary<int, Packet> Configuration => configuration ??= Packet.Deserialize(node["configuration"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? configuration;

        public FrozenDictionary<int, Packet> Play => play ??= Packet.Deserialize(node["play"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? play;
    }

    public static string Name => "protocol";

    public ServerMetadata Server => server ??= new ServerMetadata(node.AsObject());

    private ServerMetadata? server;

    public ClientMetadata Client => client ??= new ClientMetadata(node.AsObject());

    private ClientMetadata? client;

    public static Protocol Create(JsonNode node)
    {
        return new Protocol(node);
    }
}