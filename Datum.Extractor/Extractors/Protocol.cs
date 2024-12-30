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
    /// <param name="Name">Gets the packet's name.</param>
    /// <param name="Properties">Gets the packet's properties.</param>
    public sealed record Packet(string Name, Property[] Properties)
    {
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
    /// <param name="Name">Gets the property's name.</param>
    /// <param name="Type">Gets the property's value type.</param>
    public sealed record Property(string Name, string Type);

    /// <summary>
    /// Represents the packets' direction.
    /// </summary>
    public sealed class Direction(JsonObject node, string direction)
    {
        /// <summary>
        /// Gets the handshake packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Handshake => handshake ??= Packet.Extract(node["handshaking"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? handshake;

        /// <summary>
        /// Gets the status packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Status => status ??= Packet.Extract(node["status"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? status;

        /// <summary>
        /// Gets the login packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Login => login ??= Packet.Extract(node["login"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? login;

        /// <summary>
        /// Gets the configuration packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Configuration => configuration ??= Packet.Extract(node["configuration"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? configuration;

        /// <summary>
        /// Gets the play packets.
        /// </summary>
        public FrozenDictionary<int, Packet> Play => play ??= Packet.Extract(node["play"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? play;
    }

    /// <summary>
    /// Gets the name of the protocol.
    /// </summary>
    public static string Name => "protocol";

    /// <summary>
    /// Gets the server metadata.
    /// </summary>
    public Direction Server => server ??= new Direction(node.AsObject(), "toClient");

    private Direction? server;

    /// <summary>
    /// Gets the client metadata.
    /// </summary>
    public Direction Client => client ??= new Direction(node.AsObject(), "toServer");

    private Direction? client;

    private readonly JsonNode node;

    private Protocol(JsonNode node) => this.node = node;

    /// <summary>
    /// Creates a new instance of the <see cref="Protocol"/> class.
    /// </summary>
    /// <param name="node">The JSON node representing the protocol.</param>
    /// <returns>A new instance of the <see cref="Protocol"/> class.</returns>
    public static Protocol Create(JsonNode node) => new(node);
}