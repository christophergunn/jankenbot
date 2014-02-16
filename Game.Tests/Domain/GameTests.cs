using NUnit.Framework;

namespace Game.Tests.Domain
{
    [TestFixture]
    public class GameTests
    {
        private Game _game;

        [SetUp]
        public void PerTestSetup()
        {
            _game = new Game();
        }

        [Test]
        public void CanCreateGame()
        {
        }

        [Test]
        [TestCase(Move.Rock)]
        [TestCase(Move.Paper)]
        [TestCase(Move.Scissors)]
        public void GivenSameMoves_NeitherPlayerWins(Move playerOneAndTwoMove)
        {
            _game.PlayMoves(playerOneAndTwoMove, playerOneAndTwoMove);

            Assert.That(_game.LastResult, Is.EqualTo(RoundResult.Neither));
        }

        [Test]
        [TestCase(Move.Rock, Move.Paper, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Rock, Move.Scissors, RoundResult.PlayerOneWins)]
        [TestCase(Move.Paper, Move.Scissors, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Paper, Move.Rock, RoundResult.PlayerOneWins)]
        [TestCase(Move.Scissors, Move.Rock, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Scissors, Move.Paper, RoundResult.PlayerOneWins)]
        public void GivenStandardMoves_CanDetermineWinner(Move playerOneMove, Move playerTwoMove, RoundResult expectedResult)
        {
            _game.PlayMoves(playerOneMove, playerTwoMove);

            Assert.That(_game.LastResult, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(Move.Dynamite, Move.Paper, RoundResult.PlayerOneWins)]
        [TestCase(Move.Dynamite, Move.Scissors, RoundResult.PlayerOneWins)]
        [TestCase(Move.Dynamite, Move.Rock, RoundResult.PlayerOneWins)]
        [TestCase(Move.Paper, Move.Dynamite, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Scissors, Move.Dynamite, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Rock, Move.Dynamite, RoundResult.PlayerTwoWins)]
        public void GivenDynamite_ItBeatsAllStandardMoves(Move playerOneMove, Move playerTwoMove,
                                                          RoundResult expectedResult)
        {
            _game.PlayMoves(playerOneMove, playerTwoMove);

            Assert.That(_game.LastResult, Is.EqualTo(expectedResult));
        }


        [Test]
        [TestCase(Move.Waterbomb, Move.Paper, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Waterbomb, Move.Scissors, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Waterbomb, Move.Rock, RoundResult.PlayerTwoWins)]
        [TestCase(Move.Waterbomb, Move.Dynamite, RoundResult.PlayerOneWins)]
        [TestCase(Move.Paper, Move.Waterbomb, RoundResult.PlayerOneWins)]
        [TestCase(Move.Scissors, Move.Waterbomb, RoundResult.PlayerOneWins)]
        [TestCase(Move.Rock, Move.Waterbomb, RoundResult.PlayerOneWins)]
        [TestCase(Move.Dynamite, Move.Waterbomb, RoundResult.PlayerTwoWins)]
        public void GivenWaterbomb_ItBeatsNothingExceptDynamite(Move playerOneMove, Move playerTwoMove,
                                                          RoundResult expectedResult)
        {
            _game.PlayMoves(playerOneMove, playerTwoMove);

            Assert.That(_game.LastResult, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GivenPlayerOneWins_ScoreIsIncrementedByOne()
        {
            _game.PlayMoves(Move.Waterbomb, Move.Dynamite);

            Assert.That(_game.PlayerOneScore, Is.EqualTo(1));
        }

        [Test]
        public void GivenPlayerTwoWins_ScoreIsIncrementedByOne()
        {
            _game.PlayMoves(Move.Rock, Move.Paper);

            Assert.That(_game.PlayerTwoScore, Is.EqualTo(1));
        }

        [Test]
        public void GivenADraw_NeitherScoreChanges()
        {
            int p1 = _game.PlayerOneScore, p2 = _game.PlayerTwoScore;

            _game.PlayMoves(Move.Rock, Move.Rock);

            Assert.That(_game.PlayerOneScore, Is.EqualTo(p1));
            Assert.That(_game.PlayerTwoScore, Is.EqualTo(p2));
        }

        [Test]
        public void GivenRoundLimitIsSetButNotReached_GameIsNotFinishedAndFinalStateIsNotSet()
        {
            _game.SetRoundLimit(5);

            _game.PlayMoves(Move.Dynamite, Move.Rock);
            _game.PlayMoves(Move.Dynamite, Move.Rock);
            _game.PlayMoves(Move.Dynamite, Move.Rock);
            Assert.That(!_game.IsFinished);
            Assert.That(_game.FinalState, Is.Null);            
        }

        [Test] // winning
        public void GivenRoundLimitIsReached_GameIsMarkedAsFinishedAndFinalStateIsKnown()
        {
            _game.SetRoundLimit(5);

            _game.PlayMoves(Move.Dynamite, Move.Rock);
            _game.PlayMoves(Move.Dynamite, Move.Rock);
            _game.PlayMoves(Move.Dynamite, Move.Rock);
            _game.PlayMoves(Move.Dynamite, Move.Rock);
            _game.PlayMoves(Move.Dynamite, Move.Rock);

            Assert.That(_game.IsFinished);
            Assert.That(_game.FinalState, Is.EqualTo(RoundResult.PlayerOneWins));
        }

        //[Test] // expect ex if initialization funcs are called after game play commenced
        [Test]
        public void CanSetDynamiteLimit_UsageOfDynamiteWillDecrementPlayersRemainingDynamite()
        {
            _game.SetDynamiteLimit(10);
            Assert.That(_game.PlayerOneRemainingDynamite, Is.EqualTo(10));
            Assert.That(_game.PlayerTwoRemainingDynamite, Is.EqualTo(10));

            _game.PlayMoves(Move.Dynamite, Move.Rock);
            Assert.That(_game.PlayerOneRemainingDynamite, Is.EqualTo(9));

            _game.PlayMoves(Move.Rock, Move.Dynamite);
            _game.PlayMoves(Move.Rock, Move.Dynamite);
            Assert.That(_game.PlayerTwoRemainingDynamite, Is.EqualTo(8));
        }

        [Test]
        public void CanSetDynamiteLimit_WhenReachedWillThrowWaterbomb()
        {
            _game.SetDynamiteLimit(2);

            _game.PlayMoves(Move.Dynamite, Move.Rock);
            _game.PlayMoves(Move.Dynamite, Move.Rock);
            _game.PlayMoves(Move.Dynamite, Move.Rock);

            Assert.That(_game.LastResult, Is.EqualTo(RoundResult.PlayerTwoWins));
            Assert.That(_game.PlayerOneScore, Is.EqualTo(2));
            Assert.That(_game.PlayerTwoScore, Is.EqualTo(1));
        }
    }
}
