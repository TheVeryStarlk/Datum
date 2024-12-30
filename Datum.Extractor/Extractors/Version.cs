﻿using System.Text.Json.Nodes;

namespace Datum.Extractor.Extractors;

/// <summary>
/// Represents a version extractor.
/// </summary>
public sealed class Version : IExtractor<Version>
{
    /// <summary>
    /// Gets the name of the version extractor.
    /// </summary>
    public static string Name => "version";

    /// <summary>
    /// Gets or sets the version number.
    /// </summary>
    public required int Number { get; init; }

    /// <summary>
    /// Gets or sets the major version.
    /// </summary>
    public required string Major { get; init; }

    /// <summary>
    /// Gets or sets the named version.
    /// </summary>
    public required string Named { get; init; }

    /// <summary>
    /// Creates an instance of the <see cref="Version"/> class from a JSON node.
    /// </summary>
    /// <param name="node">The JSON node representing the version.</param>
    /// <returns>An instance of the <see cref="Version"/> class.</returns>
    public static Version Create(JsonNode node)
    {
        return new Version
        {
            Number = int.Parse(node["version"]!.ToString()),
            Major = node["majorVersion"]!.ToString(),
            Named = node["minecraftVersion"]!.ToString()
        };
    }
}