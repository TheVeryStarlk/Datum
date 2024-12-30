using Datum.Extractor;

var datum = await DatumExtractor.ExtractJavaAsync("1.8", CancellationToken.None);

var block = datum.Block;

var air = block!.Blocks[0];

var protocol = datum.Protocol?.Client.Handshake.First();

return;