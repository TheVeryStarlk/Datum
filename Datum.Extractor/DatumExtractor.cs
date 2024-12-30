using System.Text.Json.Nodes;

namespace Datum.Extractor;

/// <summary>
/// Provides ways to extract Minecraft information.
/// </summary>
public static class DatumExtractor
{
    private static readonly string Folder = "Source/data/".FixPathSeparator();

    public static async Task<JavaDatum> ExtractJavaAsync(string version, CancellationToken cancellationToken)
    {
        var features = await ExtractAsync(version, "pc", cancellationToken);

        return new JavaDatum(Folder, features);
    }

    public static async Task<BedrockDatum> ExtractBedrockAsync(string version, CancellationToken cancellationToken)
    {
        var features = await ExtractAsync(version, "bedrock", cancellationToken);

        return new BedrockDatum(Folder, features);
    }

    private static async Task<IDictionary<string, JsonNode?>> ExtractAsync(string version, string name, CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(Path.Join(Folder, "dataPaths.json"));

        var parent = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken);

        var type = parent![name]!.AsObject();

        if (!type.ContainsKey(version))
        {
            throw new DatumException("The associated version and edition do not exist.");
        }

        return type[version]!.AsObject();
    }
}