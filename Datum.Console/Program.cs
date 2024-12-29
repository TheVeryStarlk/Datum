using Datum.Extractor;
using Datum.Extractor.Extractors;

var datum = await DatumExtractor.ExtractAsync("1.8", Edition.Java, CancellationToken.None);

Console.WriteLine("b");

var protocol = datum.Protocol;

Console.WriteLine("a");