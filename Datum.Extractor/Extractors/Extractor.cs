using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors;

/// <summary>
/// Defines an interface for extractors.
/// </summary>
/// <typeparam name="T">The type of the extractor.</typeparam>
public interface IExtractor<out T> where T : IExtractor<T>
{
    /// <summary>
    /// Gets the name of the source file.
    /// </summary>
    public static abstract string Name { get; }

    /// <summary>
    /// Creates an instance of the extractor.
    /// </summary>
    /// <param name="node">The JSON node representing the extractor.</param>
    /// <returns>An instance of the extractor.</returns>
    public static abstract T Create(JsonNode node);
}