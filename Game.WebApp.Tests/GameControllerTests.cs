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
        private WebController.EventCoOrdinator _controller;
        private ITournament _tournament;

        [SetUp]
        public void Setup()
        {
            _tournament = Substitute.For<ITournament>();
            var per = Substitute.For<ITournamentPersistence>();
            per.LoadTournament().Returns(_tournament);

            var channelFactory = Substitute.For<IOutgoingPlayerChannelFactory>();
            var applicationConfiguration = Substitute.For<IApplicationConfiguration>();
            var actionScheduler = Substitute.For<WebController.IActionScheduler>();
            
            _controller = new WebController.EventCoOrdinator(per, channelFactory, applicationConfiguration, actionScheduler);
        }

        [Test]
        public void Start_ShouldRegisterAtLeastOneHouseBot()
        {
            var counter = 0;
            _tournament
                .When(x => x.RegisterPlayer(Arg.Any<TournamentPlayer>()))
                .Do(x => counter++);

            _controller.Start();

            Assert.That(counter, Is.GreaterThan(0));
        }

        [Test]
        public void Register_AfterStarting_ShouldDelegateToTournament()
        {
            _controller.Start();
            _controller.Register("id", "name", "ip");

            _tournament.ReceivedWithAnyArgs().RegisterPlayer(null);
        }
    }
}
