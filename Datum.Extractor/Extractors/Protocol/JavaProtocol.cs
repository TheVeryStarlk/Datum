using System.Collections.Frozen;
using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors.Protocol;

public sealed class JavaProtocol : IProtocol<JavaProtocol>
{
    /// <summary>
    /// Represents the packets' direction.
    /// </summary>
    public sealed class Direction(JsonObject node, string direction)
    {
        /// <summary>
        /// Gets the handshake packets.
        /// </summary>
        public FrozenDictionary<int, Packet>? Handshake => handshake ??= Packet.Extract(node["handshaking"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? handshake;

        /// <summary>
        /// Gets the status packets.
        /// </summary>
        public FrozenDictionary<int, Packet>? Status => status ??= Packet.Extract(node["status"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? status;

        /// <summary>
        /// Gets the login packets.
        /// </summary>
        public FrozenDictionary<int, Packet>? Login => login ??= Packet.Extract(node["login"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? login;

        /// <summary>
        /// Gets the configuration packets.
        /// </summary>
        public FrozenDictionary<int, Packet>? Configuration => configuration ??= Packet.Extract(node["configuration"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? configuration;

        /// <summary>
        /// Gets the play packets.
        /// </summary>
        public FrozenDictionary<int, Packet>? Play => play ??= Packet.Extract(node["play"]?[direction]?["types"]?.AsObject());

        private FrozenDictionary<int, Packet>? play;
    }

    /// <inheritdoc cref="IProtocol{T}"/>
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

    private JavaProtocol(JsonNode node) => this.node = node;

    public static JavaProtocol Create(JsonNode node) => new(node);
}