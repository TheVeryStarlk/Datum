using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors;

public interface IExtractor<out T> where T : IExtractor<T>
{
    public static abstract string Name { get; }

    public static abstract T Create(JsonNode node);
}