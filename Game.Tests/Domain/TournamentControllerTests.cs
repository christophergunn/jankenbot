using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Game.Utilities;

namespace Game.Tests.Domain
{
    [TestFixture]
    public class TournamentControllerTests
    {
        private IMatchMaker _matchMaker;
        private TournamentController _tournament;

        [SetUp]
        public void PerTestSetup()
        {
            _matchMaker = Substitute.For<IMatchMaker>();
            _tournament = new TournamentController(_matchMaker);
        }

        [Test]
        public void CanRegisterPlayers_AndRetrieveTheirDetailsLater()
        {
            var playerOneProxy = Substitute.For<IPlayerCommunicationChannel>();
            var playerOneName = "Alex";
            var playerOneId = Guid.NewGuid().ToString();
            var playerOne = new TournamentPlayer(playerOneId, playerOneProxy) { Name = playerOneName };

            _tournament.RegisterPlayer(playerOne);

            Assert.That(_tournament.Players.Count(), Is.EqualTo(1));
            Assert.That(_tournament.Players.First().Id, Is.EqualTo(playerOneId));
        }

        [Test]
        public void GivenTheSamePlayerIdIsRegisteredMultipleTimes_ThenTheNameIsUpdated()
        {
            var playerOneProxy = Substitute.For<IPlayerCommunicationChannel>();
            var playerOneName = "Alex";
            var playerOneId = Guid.NewGuid().ToString();
            var playerOne = new TournamentPlayer(playerOneId, playerOneProxy) { Name = playerOneName };

            _tournament.RegisterPlayer(playerOne);
            var playerOneSecondTime = new TournamentPlayer(playerOneId, playerOneProxy) { Name = "Bob" };
            _tournament.RegisterPlayer(playerOneSecondTime);

            Assert.That(_tournament.Players.Count(), Is.EqualTo(1));
            Assert.That(_tournament.Players.First().Name, Is.EqualTo("Bob"));
        }

        [Test]
        public void TournamentRulesCanBeSet()
        {
            _tournament.Setup(new TournamentConfiguration { NumberOfRounds = 2, TurnsPerRound = 1000, DynamiteLimit = null });
        }

        [Test]
        public void GivenARoundBegins_ThenMatchMakerShouldBeCalledAndAllPlayersShouldBeInformedOfTheirOpponentDetails()
        {
            var registeredPlayers = CreateAndRegisterSomeRandomPlayers(10);

            _tournament.BeginRound();
            
            var recievedCalls = _matchMaker.ReceivedCalls().ToArray();
            Assert.That(recievedCalls.Count(), Is.EqualTo(1));
            Assert.That(recievedCalls.First().GetArguments().First(), Is.EquivalentTo(registeredPlayers));
        }

        private IEnumerable<TournamentPlayer> CreateAndRegisterSomeRandomPlayers(int numberOfPlayers)
        {
            var randomPlayers = CreateSomeRandomPlayers(numberOfPlayers);
            randomPlayers.ForEach(_tournament.RegisterPlayer);
            return randomPlayers;
        }

        private TournamentPlayer[] CreateSomeRandomPlayers(int numberOfPlayers)
        {
            return (from i in Enumerable.Range(0, numberOfPlayers) select CreateRandomPlayer()).ToArray();
        }

        private TournamentPlayer CreateRandomPlayer()
        {
            return new TournamentPlayer(Guid.NewGuid().ToString(), Substitute.For<IPlayerCommunicationChannel>());
        }
    }
}
