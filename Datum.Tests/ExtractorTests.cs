using Datum.Extractor;

namespace Datum.Tests;

internal sealed class ExtractorTests
{
    [Test]
    public void ThrowsOn_InvalidVersion() => Assert.ThrowsAsync<DatumException>(static async () => await DatumExtractor.ExtractJavaAsync(string.Empty, CancellationToken.None));

    [Test]
    public async Task Extracts_OldVersionConfiguration_Correctly()
    {
        var datum = await DatumExtractor.ExtractJavaAsync("1.8", CancellationToken.None);

        Assert.That(datum.Protocol, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(datum.Protocol.Server.Configuration, Is.Empty);
            Assert.That(datum.Protocol.Client.Configuration, Is.Empty);
        });
    }
}