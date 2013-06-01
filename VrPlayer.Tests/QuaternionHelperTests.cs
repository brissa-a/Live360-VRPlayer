using NUnit.Framework;
using VrPlayer.Helpers;

namespace VrPlayer.Tests
{
    [TestFixture]
    public class QuaternionHelperTests
    {
        [Test]
        public void CanReadMetadataFromFile()
        {
            var q = QuaternionHelper.EulerAnglesInDegToQuaternion(30, 10, 25);
            var e = QuaternionHelper.QuaternionToEulerAnglesInDeg(q);
            var q2 = QuaternionHelper.EulerAnglesInDegToQuaternion(e.Y, e.X, e.Z);

            Assert.That(q, Is.EqualTo(q2));
        }
    }
}