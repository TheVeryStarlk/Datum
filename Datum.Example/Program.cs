using System.Globalization;
using System.Text;
using Datum.Extractor;

var datum = await DatumExtractor.ExtractJavaAsync("1.8", CancellationToken.None);

var info = new CultureInfo("en-US", false).TextInfo;

foreach (var pair in datum.Protocol!.Server.Play!)
{
    var packet = pair.Value;
    var name = $"{info.ToTitleCase(packet.Name[7..].Replace("_", " ")).Replace(" ", "")}Packet";
    var builder = new StringBuilder();

    builder.AppendLine($"internal sealed class {name} : IPacket");
    builder.AppendLine("{");
    builder.AppendLine($"    public int Identifier => 0x{pair.Key:X2};");

    builder.AppendLine();

    foreach (var property in packet.Properties)
    {
        var type = property.Type switch
        {
            "varint" => "int",
            "varlong" => "long",
            "pstring" or "string" => "string",
            "u16" => "ushort",
            "u8" => "byte",
            "i64" => "long",
            "buffer" => "ReadOnlyMemory<byte>",
            "i32" => "int",
            "i8" => "sbyte",
            "i16" => "short",
            "f32" => "float",
            "f64" => "double",
            "UUID" => "Guid",
            "bool" => "bool",
            _ => info.ToTitleCase(property.Type).Replace("_", "")
        };

        builder.AppendLine($"    public {type} {info.ToTitleCase(property.Name)} {{ get; set; }}");
    }

    builder.AppendLine("}");

    Console.WriteLine(builder.ToString());
}