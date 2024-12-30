using Datum.Extractor;

var datum = await DatumExtractor.ExtractBedrockAsync("1.21.50", CancellationToken.None);

var block = datum.Block;

var air = block!.Blocks[0];

var protocol = datum.Protocol?.Packets;

return;