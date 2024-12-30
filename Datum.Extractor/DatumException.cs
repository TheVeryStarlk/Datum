namespace Datum.Extractor;

/// <summary>
/// Represents an exception specific to the Datum extractor.
/// </summary>
public sealed class DatumException : Exception
{
    internal DatumException(string message) : base(message)
    {
    }
}