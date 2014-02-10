﻿namespace Game
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
        Dynamite,
        Waterbomb
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
                if (playerTwoMove == Move.Waterbomb)
                {
                    LastResult = RoundResult.PlayerTwoWins;
                }
                else
                {
                    LastResult = RoundResult.PlayerOneWins;
                }
            }
            else if (playerTwoMove == Move.Dynamite)
            {
                if (playerOneMove == Move.Waterbomb)
                {
                    LastResult = RoundResult.PlayerOneWins;
                }
                else
                {
                    LastResult = RoundResult.PlayerTwoWins;
                }
            }
            else if (playerOneMove == Move.Waterbomb)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
            else if (playerTwoMove == Move.Waterbomb)
            {
                LastResult = RoundResult.PlayerOneWins;
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
            IncrementWinningPlayersScore();
        }

        private void IncrementWinningPlayersScore()
        {
            if (LastResult == RoundResult.PlayerOneWins) PlayerOneScore++;
            if (LastResult == RoundResult.PlayerTwoWins) PlayerTwoScore++;
        }

        public int PlayerOneScore { get; private set; }

        public int PlayerTwoScore { get; private set; }

        public RoundResult? LastResult
        {
            get; private set; 
        }
    }
}
