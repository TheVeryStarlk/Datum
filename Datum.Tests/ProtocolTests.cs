using Datum.Extractor;

namespace Datum.Tests;

internal sealed class ProtocolTests
{
    [Test]
    public async Task Extracts_AllStates_Correctly()
    {
        var datum = await DatumExtractor.ExtractJavaAsync("1.8", CancellationToken.None);

        Assert.That(datum.Protocol, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(datum.Protocol.Server.Handshake, Is.Null);
            Assert.That(datum.Protocol.Server.Login, Is.Not.Empty);
            Assert.That(datum.Protocol.Server.Configuration, Is.Null);
            Assert.That(datum.Protocol.Server.Play, Is.Not.Empty);

            Assert.That(datum.Protocol.Client.Handshake, Is.Not.Empty);
            Assert.That(datum.Protocol.Client.Login, Is.Not.Empty);
            Assert.That(datum.Protocol.Client.Configuration, Is.Null);
            Assert.That(datum.Protocol.Client.Play, Is.Not.Empty);
        });
    }
}