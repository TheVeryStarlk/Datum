using System.Text.Json.Nodes;

namespace Datum.Extractor;

public static class DatumExtractor
{
    public static string Folder { get; } = "Source/data/".FixPathSeparator();

    public static async Task<Datum> ExtractAsync(string version, Edition edition, CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(Path.Join(Folder, "dataPaths.json"));

        var parent = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken);

        var type = parent![edition is Edition.Java ? "pc" : "bedrock"]!.AsObject();

        if (!type.ContainsKey(version))
        {
            throw new DatumException("The associated version and edition do not exist.");
        }

        IDictionary<string, JsonNode?> features = type[version]!.AsObject();

        return new Datum(features);
    }
}