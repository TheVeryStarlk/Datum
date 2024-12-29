using System.Text.Json.Nodes;

namespace Datum.Extractor;

public static class DatumExtractor
{
    public static async Task<Datum> ExtractAsync(string version, Edition edition, CancellationToken cancellationToken)
    {
        const string path = "Source/data/dataPaths.json";

        await using var stream = File.OpenRead(path);

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