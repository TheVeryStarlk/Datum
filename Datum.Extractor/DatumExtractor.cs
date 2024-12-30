using System.Text.Json.Nodes;

namespace Datum.Extractor;

/// <summary>
/// Provides methods for extracting Minecraft data.
/// </summary>
public static class DatumExtractor
{
    /// <summary>
    /// Asynchronously extracts data for the specified version and edition.
    /// </summary>
    /// <param name="version">The version of the data.</param>
    /// <param name="edition">The edition of the data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous extract operation.</returns>
    /// <exception cref="DatumException">Thrown when the associated version and edition do not exist.</exception>
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