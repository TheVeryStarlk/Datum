namespace Datum.Extractor;

/// <summary>
/// Represents an exception specific to the Datum extractor.
/// </summary>
public sealed class DatumException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DatumException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    internal DatumException(string message) : base(message)
    {
    }
}