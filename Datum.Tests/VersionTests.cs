using Datum.Extractor;

namespace Datum.Tests;

internal sealed class VersionTests
{
    [Test]
    public async Task Extracts_Version_Correctly()
    {
        var datum = await DatumExtractor.ExtractJavaAsync("1.8", CancellationToken.None);

        Assert.That(datum.Version, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(datum.Version.Number, Is.EqualTo(47));
            Assert.That(datum.Version.Major, Is.EqualTo("1.8"));
            Assert.That(datum.Version.Named, Is.EqualTo("1.8.8"));
        });
    }
}