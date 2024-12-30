using Datum.Extractor;

namespace Datum.Tests;

internal sealed class ExtractorTests
{
    [Test]
    public void ThrowsOn_InvalidVersion()
    {
        Assert.ThrowsAsync<DatumException>(static async () => await DatumExtractor.ExtractAsync(string.Empty, Edition.Java, CancellationToken.None));
    }

    [Test]
    public async Task Extracts_OldVersionConfiguration_Correctly()
    {
        var datum = await DatumExtractor.ExtractAsync("1.8", Edition.Java, CancellationToken.None);

        Assert.That(datum.Protocol, Is.Not.EqualTo(null));

        Assert.Multiple(() =>
        {
            Assert.That(datum.Protocol.Server.Configuration, Is.Empty);
            Assert.That(datum.Protocol.Client.Configuration, Is.Empty);
        });
    }
}