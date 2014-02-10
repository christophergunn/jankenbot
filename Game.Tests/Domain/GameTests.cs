using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
