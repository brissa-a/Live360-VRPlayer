using NUnit.Framework;
using VrPlayer.Models.Metadata;

namespace VrPlayer.Tests
{
    [TestFixture]
    public class MetadataTests
    {
        [Test]
        public void CanReadMetadataFromFile()
        {
            var parser = new MetadataParser(@"TestData\FileWithXmpMetadata.jpg");
            var metadata = parser.Parse();

            Assert.That(metadata.FormatType, Is.EqualTo("OverUnder"));
            Assert.That(metadata.ProjectionType, Is.EqualTo("VrPlayer.Projections.DomeProjection"));
        }

        [Test]
        public void CanReadFileWhitoutMetadata()
        {
            var parser = new MetadataParser(@"TestData\FileWithoutXmpMetadata.jpg");

            Assert.DoesNotThrow(() => parser.Parse());
        }
    }
}