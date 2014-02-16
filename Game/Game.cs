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
        Dynamite,
        Waterbomb
    }

    public class Game
    {
        private int _roundLimit;
        private int _roundsPlayed;
        private int? _dynamiteLimit;
        private Move _currentPlayerOneMove;
        private Move _currentPlayerTwoMove;

        public int PlayerOneScore { get; private set; }

        public int PlayerTwoScore { get; private set; }

        public int PlayerOneRemainingDynamite { get; private set; }

        public int PlayerTwoRemainingDynamite { get; private set; }

        public RoundResult? LastResult { get; private set; }

        public bool IsFinished { get; private set; }

        public RoundResult? FinalState { get; private set; }

        public void SetRoundLimit(int rounds)
        {
            _roundLimit = rounds;
        }

        public void SetDynamiteLimit(int limit)
        {
            _dynamiteLimit = limit;
            PlayerOneRemainingDynamite = limit;
            PlayerTwoRemainingDynamite = limit;
        }

        public void PlayMoves(Move playerOneMove, Move playerTwoMove)
        {
            _currentPlayerOneMove = playerOneMove;
            _currentPlayerTwoMove = playerTwoMove;

            HandleDynamiteStock();
            SetLastResult();
            IncrementWinningPlayersScore();
            UpdateFinishState();
        }

        private void SetLastResult()
        {
            if (_currentPlayerOneMove == _currentPlayerTwoMove)
            {
                LastResult = RoundResult.Neither;
            }
            else if (_currentPlayerOneMove == Move.Dynamite)
            {
                if (_currentPlayerTwoMove == Move.Waterbomb)
                {
                    LastResult = RoundResult.PlayerTwoWins;
                }
                else
                {
                    LastResult = RoundResult.PlayerOneWins;
                }
            }
            else if (_currentPlayerTwoMove == Move.Dynamite)
            {
                if (_currentPlayerOneMove == Move.Waterbomb)
                {
                    LastResult = RoundResult.PlayerOneWins;
                }
                else
                {
                    LastResult = RoundResult.PlayerTwoWins;
                }
            }
            else if (_currentPlayerOneMove == Move.Waterbomb)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
            else if (_currentPlayerTwoMove == Move.Waterbomb)
            {
                LastResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Rock && _currentPlayerTwoMove == Move.Paper)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
            else if (_currentPlayerOneMove == Move.Rock && _currentPlayerTwoMove == Move.Scissors)
            {
                LastResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Paper && _currentPlayerTwoMove == Move.Rock)
            {
                LastResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Paper && _currentPlayerTwoMove == Move.Scissors)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
            else if (_currentPlayerOneMove == Move.Scissors && _currentPlayerTwoMove == Move.Paper)
            {
                LastResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Scissors && _currentPlayerTwoMove == Move.Rock)
            {
                LastResult = RoundResult.PlayerTwoWins;
            }
        }

        private void HandleDynamiteStock()
        {
            if (!_dynamiteLimit.HasValue) return;
            if (_currentPlayerOneMove == Move.Dynamite && PlayerOneRemainingDynamite == 0)
            {
                _currentPlayerOneMove = Move.Waterbomb;
                return;
            }
            if (_currentPlayerTwoMove == Move.Dynamite && PlayerTwoRemainingDynamite == 0)
            {
                _currentPlayerTwoMove = Move.Waterbomb;
                return;
            }

            ReduceDynamiteStock();
        }

        private void ReduceDynamiteStock()
        {
            if (_currentPlayerOneMove == Move.Dynamite) PlayerOneRemainingDynamite--;
            if (_currentPlayerTwoMove == Move.Dynamite) PlayerTwoRemainingDynamite--;
        }

        private void UpdateFinishState()
        {
            _roundsPlayed++;
            if (_roundsPlayed == _roundLimit)
            {
                IsFinished = true;
                if (PlayerOneScore == PlayerTwoScore) FinalState = RoundResult.Neither;
                else if (PlayerOneScore > PlayerTwoScore) FinalState = RoundResult.PlayerOneWins;
                else FinalState = RoundResult.PlayerTwoWins;
            }
        }

        private void IncrementWinningPlayersScore()
        {
            if (LastResult == RoundResult.PlayerOneWins) PlayerOneScore++;
            if (LastResult == RoundResult.PlayerTwoWins) PlayerTwoScore++;
        }
    }
}
