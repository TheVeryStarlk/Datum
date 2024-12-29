using Datum.Extractor;

var datum = await DatumExtractor.ExtractAsync("1.21.3", Edition.Java, CancellationToken.None);

var handshake = datum.Protocol.Client.Handshake;