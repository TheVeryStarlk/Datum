using Datum.Extractor;
using Datum.Extractor.Extractors;

var datum = await DatumExtractor.ExtractAsync("1.8", Edition.Java, CancellationToken.None);

var protocol = await datum.ExtractAsync<Protocol>(CancellationToken.None);