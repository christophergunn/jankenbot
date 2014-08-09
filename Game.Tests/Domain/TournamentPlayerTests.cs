using NSubstitute;
using NUnit.Framework;

namespace Game.Tests.Domain
{
    [TestFixture]
    public class TournamentPlayerTests
    {
        [Test]
        public void Create_ShouldHaveNonNullSCoreObject()
        {
            var comms = Substitute.For<IPlayerCommunicationChannel>();

            var tp = new TournamentPlayer(null, null);

            Assert.That(tp.Score, Is.Not.Null);
        }
    }
}