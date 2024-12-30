using System.Collections.Frozen;
using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors;

/// <summary>
/// Represents a block extractor.
/// </summary>
public sealed class Block : IExtractor<Block>
{
    /// <summary>
    /// Represents the metadata of a block.
    /// </summary>
    public sealed class BlockMetadata
    {
        /// <summary>
        /// Gets or sets the identifier of the block.
        /// </summary>
        public required int Identifier { get; init; }

        /// <summary>
        /// Gets or sets the display name of the block.
        /// </summary>
        public required string? DisplayName { get; init; }

        /// <summary>
        /// Gets or sets the friendly name of the block.
        /// </summary>
        public required string? FriendlyName { get; init; }

        /// <summary>
        /// Gets or sets the stack size of the block.
        /// </summary>
        public required int StackSize { get; init; }

        /// <summary>
        /// Gets or sets the hardness of the block.
        /// </summary>
        public required float Hardness { get; init; }

        /// <summary>
        /// Gets or sets the resistance of the block.
        /// </summary>
        public required float Resistance { get; init; }

        /// <summary>
        /// Gets or sets the light strength of the block.
        /// </summary>
        public required float LightStrength { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the block is diggable.
        /// </summary>
        public required bool IsDiggable { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether the block is transparent.
        /// </summary>
        public required bool IsTransparent { get; init; }

        /// <summary>
        /// Extracts the block metadata from a JSON array.
        /// </summary>
        /// <param name="node">The JSON array representing the block metadata.</param>
        /// <returns>A frozen dictionary of block metadata.</returns>
        internal static FrozenDictionary<int, BlockMetadata> Extract(JsonArray node)
        {
            var blocks = new Dictionary<int, BlockMetadata>();

            foreach (var item in node)
            {
                var block = new BlockMetadata
                {
                    Identifier = int.Parse(item!["id"]?.ToString()!),
                    DisplayName = item["displayName"]?.ToString(),
                    FriendlyName = item["name"]?.ToString(),
                    StackSize = int.TryParse(item["stackSize"]?.ToString(), out var stackSize) ? stackSize : 0,
                    Hardness = float.TryParse(item["hardness"]?.ToString(), out var hardness) ? hardness : 0,
                    Resistance = float.TryParse(item["resistance"]?.ToString(), out var resistance) ? resistance : 0,
                    LightStrength = float.TryParse(item["emitLight"]?.ToString(), out var lightStrength) ? lightStrength : 0,
                    IsDiggable = bool.TryParse(item["diggable"]?.ToString(), out var isDiggable) && isDiggable,
                    IsTransparent = bool.TryParse(item["transparent"]?.ToString(), out var isTransparent) && isTransparent
                };

                blocks.Add(block.Identifier, block);
            }

            return blocks.ToFrozenDictionary();
        }
    }

    /// <summary>
    /// Gets the name of the block extractor.
    /// </summary>
    public static string Name => "blocks";

    /// <summary>
    /// Gets the blocks metadata.
    /// </summary>
    public FrozenDictionary<int, BlockMetadata> Blocks => blocks ??= BlockMetadata.Extract(node.AsArray());

    private FrozenDictionary<int, BlockMetadata>? blocks;

    private readonly JsonNode node;

    /// <summary>
    /// Initializes a new instance of the <see cref="Block"/> class.
    /// </summary>
    /// <param name="node">The JSON node representing the block.</param>
    public Block(JsonNode node)
    {
        this.node = node;
    }

    /// <summary>
    /// Creates an instance of the <see cref="Block"/> class from a JSON node.
    /// </summary>
    /// <param name="node">The JSON node representing the block.</param>
    /// <returns>An instance of the <see cref="Block"/> class.</returns>
    public static Block Create(JsonNode node)
    {
        return new Block(node);
    }
}