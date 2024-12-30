using System.Globalization;
using System.Text;
using Datum.Extractor;

const string source = """
                          internal interface IBlock
                          {
                              /// <summary>
                              /// Gets the numerical identifier of the block.
                              /// </summary>
                              public static abstract int Identifier { get; }
                          
                              /// <summary>
                              /// Gets the friendly name of the block.
                              /// </summary>
                              public static abstract string FriendlyName { get; }
                          
                              /// <summary>
                              /// Gets the maximum item stack size of the block.
                              /// </summary>
                              public static abstract int StackSize { get; }
                          
                              /// <summary>
                              /// Gets the mining hardness of the block.
                              /// </summary>
                              public static abstract float Hardness { get; }
                          
                              /// <summary>
                              /// Gets the block's resistance to explosives.
                              /// </summary>
                              public static abstract float Resistance { get; }
                          
                              /// <summary>
                              /// Gets the block's light strength.
                              /// </summary>
                              /// <remarks>
                              /// Blocks that do not emit light have light strength of 0.
                              /// </remarks>
                              public static abstract float LightStrength { get; }
                          
                              /// <summary>
                              /// Gets if the block is diggable or not.
                              /// </summary>
                              public static abstract bool IsDiggable { get; }
                          
                              /// <summary>
                              /// Gets whether the block is partially or fully transparent.
                              /// </summary>
                              public static abstract bool IsTransparent { get; }
                          }
                          """;

var datum = await DatumExtractor.ExtractJavaAsync("1.8", CancellationToken.None);

var builder = new StringBuilder();

builder.AppendLine(source);
builder.AppendLine();

builder.AppendLine("internal abstract class Block");
builder.AppendLine("{");

var text = new CultureInfo("en_US", false).TextInfo;

foreach (var pair in datum.Block!.Blocks)
{
    var name = pair.Value.FriendlyName!;

    name = text
        .ToTitleCase(name.Replace("(", " ").Replace(")", ""))
        .Replace(" ", "")
        .Replace("'", "")
        .Replace("/", "");

    builder.AppendLine($"   public sealed class {name} : IBlock");
    builder.AppendLine("   {");

    var body = $"""
                       public static int Identifier => {pair.Key};
               
                       public static string FriendlyName => "{pair.Value.FriendlyName}";
               
                       public static int StackSize => {pair.Value.StackSize};
               
                       public static float Hardness => {pair.Value.Hardness}F;
               
                       public static float Resistance => {pair.Value.Resistance}F;
               
                       public static float LightStrength => {pair.Value.LightStrength}F;
               
                       public static bool IsDiggable => {pair.Value.IsDiggable.ToString().ToLower()};
               
                       public static bool IsTransparent => {pair.Value.IsTransparent.ToString().ToLower()};
               """;

    builder.AppendLine(body);
    builder.AppendLine("   }");
    builder.AppendLine();
}

builder.AppendLine("}");

Console.WriteLine(builder.ToString());