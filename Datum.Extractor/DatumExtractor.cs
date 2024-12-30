using System.Text.Json.Nodes;

namespace Datum.Extractor;

/// <summary>
/// Provides ways to extract Minecraft information.
/// </summary>
public static class DatumExtractor
{
    /// <summary>
    /// Extracts Minecraft-related information for the specified <paramref name="version"/> and <paramref name="edition"/>.
    /// </summary>
    /// <param name="version">The Minecraft's version.</param>
    /// <param name="edition">The Minecraft's edition, Java or Bedrock.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous extract operation.</returns>
    /// <exception cref="DatumException">The associated version and edition do not exist.</exception>
    public static async Task<Datum> ExtractAsync(string version, Edition edition, CancellationToken cancellationToken)
    {
        var folder = "Source/data/".FixPathSeparator();

        await using var stream = File.OpenRead(Path.Join(folder, "dataPaths.json"));

        var parent = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken);

        var type = parent![edition is Edition.Java ? "pc" : "bedrock"]!.AsObject();

        if (!type.ContainsKey(version))
        {
            throw new DatumException("The associated version and edition do not exist.");
        }

        IDictionary<string, JsonNode?> features = type[version]!.AsObject();

        return new Datum(folder, features);
    }
}