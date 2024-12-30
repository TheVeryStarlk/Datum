using Datum.Extractor;

var datum = await DatumExtractor.ExtractAsync("1.8", Edition.Java, CancellationToken.None);

var block = datum.Block;

var air = block!.Blocks[0];

var protocol = datum.Protocol;

var server = protocol!.Server;

var count = protocol.Server.Play.Count;

var play = server.Play.FirstOrDefault(packet => packet.Key is 0x00);