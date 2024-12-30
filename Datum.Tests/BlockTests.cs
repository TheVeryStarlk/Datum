using Datum.Extractor;

namespace Datum.Tests;

internal sealed class BlockTests
{
    [Test]
    public async Task Extracts_Block_Information_Correctly()
    {
        var datum = await DatumExtractor.ExtractJavaAsync("1.8", CancellationToken.None);

        Assert.That(datum.Block, Is.Not.Null);

        Assert.Multiple(() =>
        {
            var first = datum.Block.Blocks.First();
            Assert.That(first.Key, Is.EqualTo(first.Value.Identifier));
        });
    }
}