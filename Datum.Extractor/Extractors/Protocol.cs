using System.Collections.Frozen;
using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors;

/// <summary>
/// Represents a protocol extractor.
/// </summary>
public sealed class Protocol : IExtractor<Protocol>
{
    /// <summary>
    /// Represents a packet with a name and properties.
    /// </summary>
    public sealed record Packet(string Name, Property[] Properties)
    {
        /// <summary>
        /// Deserializes the JSON object into a dictionary of packets.
        /// </summary>
        /// <param name="types">The JSON object representing the packet types.</param>
        /// <returns>A frozen dictionary of packets.</returns>
        internal static FrozenDictionary<int, Packet> Extract(JsonObject? types)
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

    /// <summary>
    /// Represents a property with a name and type.
    /// </summary>
    public sealed record Property(string Name, string Type);

    /// <summary>
    /// Represents the server metadata.
    /// </summary>
    public sealed class ServerMetadata
    {
        private readonly JsonObject node;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerMetadata"/> class.
        /// </summary>
        /// <param name="node">The JSON object representing the server metadata.</param>
        public ServerMetadata(JsonObject node)
        {
            this.node = node;
        }

        /// <summary>
        /// Gets the handshake packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Handshake => handshake ??= Packet.Extract(node["handshaking"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? handshake;

        /// <summary>
        /// Gets the status packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Status => status ??= Packet.Extract(node["status"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? status;

        /// <summary>
        /// Gets the login packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Login => login ??= Packet.Extract(node["login"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? login;

        /// <summary>
        /// Gets the configuration packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Configuration => configuration ??= Packet.Extract(node["configuration"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? configuration;

        /// <summary>
        /// Gets the play packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Play => play ??= Packet.Extract(node["play"]?["toClient"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? play;
    }

    /// <summary>
    /// Represents the client metadata.
    /// </summary>
    public sealed class ClientMetadata
    {
        private readonly JsonObject node;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientMetadata"/> class.
        /// </summary>
        /// <param name="node">The JSON object representing the client metadata.</param>
        public ClientMetadata(JsonObject node)
        {
            this.node = node;
        }

        /// <summary>
        /// Gets the handshake packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Handshake => handshake ??= Packet.Extract(node["handshaking"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? handshake;

        /// <summary>
        /// Gets the status packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Status => status ??= Packet.Extract(node["status"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? status;

        /// <summary>
        /// Gets the login packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Login => login ??= Packet.Extract(node["login"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? login;

        /// <summary>
        /// Gets the configuration packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Configuration => configuration ??= Packet.Extract(node["configuration"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? configuration;

        /// <summary>
        /// Gets the play packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Play => play ??= Packet.Extract(node["play"]?["toServer"]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? play;
    }

    /// <summary>
    /// Gets the name of the protocol.
    /// </summary>
    public static string Name => "protocol";

    /// <summary>
    /// Gets the server metadata.
    /// </summary>
    public ServerMetadata Server => server ??= new ServerMetadata(node.AsObject());

    private ServerMetadata? server;

    /// <summary>
    /// Gets the client metadata.
    /// </summary>
    public ClientMetadata Client => client ??= new ClientMetadata(node.AsObject());

    private ClientMetadata? client;

    private readonly JsonNode node;

    /// <summary>
    /// Initializes a new instance of the <see cref="Protocol"/> class.
    /// </summary>
    /// <param name="node">The JSON node representing the protocol.</param>
    public Protocol(JsonNode node)
    {
        this.node = node;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Protocol"/> class.
    /// </summary>
    /// <param name="node">The JSON node representing the protocol.</param>
    /// <returns>A new instance of the <see cref="Protocol"/> class.</returns>
    public static Protocol Create(JsonNode node)
    {
        return new Protocol(node);
    }
}