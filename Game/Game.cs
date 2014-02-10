using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public enum RoundResult
    {
        PlayerOneWins,
        Neither,
        PlayerTwoWins
    }

    public enum Move
    {
        Rock,
        Paper,
        Scissors,
        Dynamite
    }

    public class Game
    {
        public void PlayMoves(Move playerOneMove, Move playerTwoMove)
        {
            if (playerOneMove == playerTwoMove)
            {
                LastResult = RoundResult.Neither;
            } 
            else if (playerOneMove == Move.Dynamite)
            {
                LastResult = RoundResult.PlayerOneWins;
            }
            else if (playerTwoMove == Move.Dynamite)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
            else if (playerOneMove == Move.Rock && playerTwoMove == Move.Paper)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
            else if (playerOneMove == Move.Rock && playerTwoMove == Move.Scissors)
            {
                LastResult = RoundResult.PlayerOneWins;
            }
            else if (playerOneMove == Move.Paper && playerTwoMove == Move.Rock)
            {
                LastResult = RoundResult.PlayerOneWins;
            }
            else if (playerOneMove == Move.Paper && playerTwoMove == Move.Scissors)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
            else if (playerOneMove == Move.Scissors && playerTwoMove == Move.Paper)
            {
                LastResult = RoundResult.PlayerOneWins;
            }
            else if (playerOneMove == Move.Scissors && playerTwoMove == Move.Rock)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
        }

        public RoundResult? LastResult
        {
            get; private set; 
        }
    }
}
