# Datum
An AOT-friendly library that provides easy to access APIs to extract Minecraft information from this [repository](https://github.com/PrismarineJS/minecraft-data).

## Getting started
Datum is currently not published on NuGet, so the best way to use it is to clone the clone or add the [extractor project](https://github.com/TheVeryStarlk/Datum/tree/main/Datum.Extractor) as a submodule.
You can see Datum's API in the [example](https://github.com/TheVeryStarlk/Datum/blob/main/Datum.Example/Program.cs) and [test](https://github.com/TheVeryStarlk/Datum/tree/main/Datum.Tests) projects.

```csharp
using Datum.Extractor;

var datum = await DatumExtractor.ExtractAsync("1.8", Edition.Java, CancellationToken.None);

var block = datum.Block;
var air = block!.Blocks.First();

var protocol = datum.Protocol;
var packet = protocol!.Client.Handshake.First();
```