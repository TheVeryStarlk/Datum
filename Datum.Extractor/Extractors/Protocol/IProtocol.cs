namespace Datum.Extractor.Extractors.Protocol;

/// <summary>
/// Represents a protocol extractor.
/// </summary>
public interface IProtocol<out T> : IExtractor<T> where T : IExtractor<T>;