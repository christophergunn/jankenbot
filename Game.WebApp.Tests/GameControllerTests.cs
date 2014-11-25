using System.Linq;
using Game.WebApp.Api;
using Game.WebApp.Configuration;
using WebController = Game.WebApp.Controller;
using NUnit.Framework;
using NSubstitute;
namespace Game.WebApp.Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        private WebController.GameController _controller;
        private TournamentController _tournament;

        [SetUp]
        private void Setup()
        {
            _tournament = Substitute.For<TournamentController>();
            var per = Substitute.For<ITournamentPersistence>();
            per.LoadTournament().Returns(_tournament);

            var channelFactory = Substitute.For<IOutgoingPlayerChannelFactory>();
            var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
            var actionScheduler = Substitute.For<WebController.IActionScheduler>();
            
            _controller = new WebController.GameController(per, channelFactory, applicationConfiguration, actionScheduler);
        }

        [Test]
        public void Start_ShouldRegisterAtLeastOneHouseBot()
        {
            _controller.Start();

            Assert.That(_controller.Tournament.Players.Count(), Is.GreaterThan(0));
        }

        [Test]
        public void Register_ShouldDelegateToTournament_IfInvalidState()
        {
            _controller.Register("id", "name", "ip");

            _tournament.ReceivedWithAnyArgs().RegisterPlayer(null);
        }
    }
}
