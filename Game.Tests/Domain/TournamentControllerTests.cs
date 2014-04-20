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
            var playerOne = new TournamentPlayer(playerOneId, playerOneName, playerOneProxy);

            _tournament.RegisterPlayer(playerOne);

            Assert.That(_tournament.Players.Count(), Is.EqualTo(1));
            Assert.That(_tournament.Players.First().Id, Is.EqualTo(playerOneId));
            Assert.That(_tournament.Players.First().Name, Is.EqualTo(playerOneName));
        }

        [Test]
        public void GivenTheSamePlayerIdIsRegisteredMultipleTimes_ThenTheNameIsUpdated()
        {
            var playerOneProxy = Substitute.For<IPlayerCommunicationChannel>();
            var playerOneName = "Alex";
            var playerOneId = Guid.NewGuid().ToString();
            var playerOne = new TournamentPlayer(playerOneId, playerOneName, playerOneProxy);

            _tournament.RegisterPlayer(playerOne);
            var playerOneSecondTime = new TournamentPlayer(playerOneId, "Bob", playerOneProxy);
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
        public void GivenARoundBegins_CurrentRoundShouldBeSetWithCurrentRegisteredPlayers()
        {
            var registeredPlayers = CreateAndRegisterSomeRandomPlayers(4);

            _tournament.BeginNewRound();

            Assert.That(_tournament.CurrentRound, Is.Not.Null);
            Assert.That(_tournament.CurrentRound.Players, Is.EquivalentTo(registeredPlayers));
        }

        [Test]
        public void GivenARoundBegins_AdditionalPlayerRegistrationsWillNotCount()
        {
            var registeredPlayers = CreateAndRegisterSomeRandomPlayers(4);

            _tournament.BeginNewRound();
            CreateAndRegisterSomeRandomPlayers(2);

            Assert.That(_tournament.CurrentRound, Is.Not.Null);
            Assert.That(_tournament.CurrentRound.Players, Is.EquivalentTo(registeredPlayers));
        }

        [Test]
        public void GivenMultipleRounds_RoundSequenceNumberShouldIncrease()
        {
            CreateAndRegisterSomeRandomPlayers(4);

            _tournament.BeginNewRound();
            Assert.That(_tournament.CurrentRound.SequenceNumber, Is.EqualTo(1));
            _tournament.BeginNewRound();
            Assert.That(_tournament.CurrentRound.SequenceNumber, Is.EqualTo(2));
        }

        [Test]
        public void GivenMultipleRounds_NewlyRegisteredPlayersWillPartakeInTheNextRound()
        {
            var registeredPlayers = CreateAndRegisterSomeRandomPlayers(4);

            _tournament.BeginNewRound();
            var lateComers = CreateAndRegisterSomeRandomPlayers(2);
            Assert.That(_tournament.CurrentRound.Players, Is.EquivalentTo(registeredPlayers));
            _tournament.BeginNewRound();
            var evenLaterComers = CreateAndRegisterSomeRandomPlayers(7);
            Assert.That(_tournament.CurrentRound.Players, Is.EquivalentTo(registeredPlayers.Union(lateComers)));
            _tournament.BeginNewRound();
            Assert.That(_tournament.CurrentRound.Players, Is.EquivalentTo(registeredPlayers.Union(lateComers).Union(evenLaterComers)));
        }

        [Test]
        public void GivenARoundBegins_ThenMatchMakerShouldBeCalledAndAllPlayersShouldBeInformedOfTheirOpponentDetails()
        {
            var registeredPlayers = CreateAndRegisterSomeRandomPlayers(4);
            _matchMaker.Matches.Returns(
                ci => new[] { new Tuple<TournamentPlayer, TournamentPlayer>(registeredPlayers[0], registeredPlayers[1]),
                              new Tuple<TournamentPlayer, TournamentPlayer>(registeredPlayers[2], registeredPlayers[3])});

            _tournament.BeginNewRound();
            
            Assert.That(_matchMaker.ReceivedCalls().First().GetArguments().First(), Is.EquivalentTo(registeredPlayers));
            AssertThatPlayersWereInformedOfTheirOpponents();
        }

        private void AssertThatPlayersWereInformedOfTheirOpponents()
        {
            _matchMaker.Matches.ForEach(AssertOpponentsWereInformedOfEachOther);
        }

        private void AssertOpponentsWereInformedOfEachOther(Tuple<TournamentPlayer, TournamentPlayer> opponents)
        {
            var p1 = opponents.Item1;
            var p2 = opponents.Item2;

            p1.Comms.Received(1).InformOfGameAgainst(p2);
            p2.Comms.Received(1).InformOfGameAgainst(p1);
        }

        [Test]
        public void GivenARoundBeings_ThenGamesShouldBeSetUpBetweenMatchedPlayersInCurrentRound()
        {
            var registeredPlayers = SetupAndBeginASixPlayerRound();

            var games = _tournament.CurrentRound.Games.ToArray();
            
            Assert.That(games.Length, Is.EqualTo(3));
            AssertThatGameIsBetweenPlayers(games[0], registeredPlayers[0], registeredPlayers[5]);
            AssertThatGameIsBetweenPlayers(games[1], registeredPlayers[1], registeredPlayers[4]);
            AssertThatGameIsBetweenPlayers(games[2], registeredPlayers[2], registeredPlayers[3]);
        }

        private void AssertThatGameIsBetweenPlayers(TournamentGame tournamentGame, TournamentPlayer p1, TournamentPlayer p2)
        {
            Assert.That(tournamentGame.PlayerOne.Id, Is.EqualTo(p1.Id));
            Assert.That(tournamentGame.PlayerTwo.Id, Is.EqualTo(p2.Id));
        }

        [Test]
        public void PlayMove_GivenAPlayer_ShouldInformGameOfMovePlayed()
        {
            var registeredPlayers = SetupAndBeginASixPlayerRound();

            _tournament.PlayMove(registeredPlayers[0], Move.Scissors, registeredPlayers[5], Move.Paper);

            Assert.That(_tournament.CurrentRound.Games.First().LastResult, Is.EqualTo(RoundResult.PlayerOneWins));
        }

        [Test]
        public void GivenAllRoundsEnd_ThenPlayersWonRoundsCountShouldBeIncrementedAppropriately()
        {
            var registeredPlayers = SetupAndBeginASixPlayerRound();

            PlayOneRoundWithWinners(registeredPlayers[0]);
            Assert.That(registeredPlayers[0].Score.WonRounds, Is.EqualTo(0));

            PlayOneRoundWithWinners(registeredPlayers[1], registeredPlayers[2]);
            Assert.That(registeredPlayers[0].Score.WonRounds, Is.EqualTo(1));
        }

        private void PlayOneRoundWithWinners(params TournamentPlayer[] winningPlayers)
        {
            foreach (var tournamentPlayer in winningPlayers)
            {
                var game = (from tr in _tournament.CurrentRound.Games
                            where tr.PlayerOne.Id == tournamentPlayer.Id || tr.PlayerTwo.Id == tournamentPlayer.Id 
                            select tr).FirstOrDefault();
                for (int i = 0; i < 1000; i++)
                {
                    _tournament.PlayMove(tournamentPlayer, Move.Scissors, (tournamentPlayer.Id == game.PlayerOne.Id ? game.PlayerTwo : game.PlayerOne), Move.Paper);
                }
            }
        }

        private TournamentPlayer[] SetupAndBeginASixPlayerRound()
        {
            var registeredPlayers = SetupASixPlayerRound();
            _tournament.BeginNewRound();
            return registeredPlayers;
        }

        private TournamentPlayer[] SetupASixPlayerRound()
        {
            var registeredPlayers = CreateAndRegisterSomeRandomPlayers(6);
            _matchMaker.Matches.Returns(
                ci => new[]
                    {
                        new Tuple<TournamentPlayer, TournamentPlayer>(registeredPlayers[0], registeredPlayers[5]),
                        new Tuple<TournamentPlayer, TournamentPlayer>(registeredPlayers[1], registeredPlayers[4]),
                        new Tuple<TournamentPlayer, TournamentPlayer>(registeredPlayers[2], registeredPlayers[3])
                    });
            return registeredPlayers;
        }

        [Test]
        public void GivenAllRoundsArePlayed_TournamentShouldBeMarkedAsFinished()
        {
            SetupAndBeginASixPlayerRound();

            foreach (var roundNum in Enumerable.Range(1, _tournament.Config.NumberOfRounds))
            {
                _tournament.PlayRound();   
            }

            Assert.That(_tournament.IsFinished, Is.True);
        }

        [Test]
        public void PlayGame_ShouldRequestMoveFromEachPlayerOncePerRound()
        {
            SetupAndBeginASixPlayerRound();

            var firstGame = _tournament.CurrentRound.Games.First();
            _tournament.PlayGame(firstGame);

            AssertAllPlayersWereAskedForMoves(firstGame, _tournament.Config.TurnsPerRound);
        }

        private void AssertAllPlayersWereAskedForMoves(TournamentGame game, int turnsPerRound)
        {
            game.PlayerOne.Comms.Received(turnsPerRound).RequestMove();
            game.PlayerTwo.Comms.Received(turnsPerRound).RequestMove();
        }

        private TournamentPlayer[] CreateAndRegisterSomeRandomPlayers(int numberOfPlayers)
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
            var id = Guid.NewGuid().ToString();
            return new TournamentPlayer(id, "Name - " + id, Substitute.For<IPlayerCommunicationChannel>());
        }
    }
}
