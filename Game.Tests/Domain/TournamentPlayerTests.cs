using NSubstitute;
using NUnit.Framework;

namespace Game.Tests.Domain
{
    [TestFixture]
    public class TournamentPlayerTests
    {
        [Test]
        public void Create_ShouldRegisterWithCommsChannel()
        {
            var comms = Substitute.For<IPlayerCommunicationChannel>();

            var tp = new TournamentPlayer(null, null, comms);

            comms.Received(1).Start(tp);
        }
    }
}
