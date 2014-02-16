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
        private int _rollingAccumulator;

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
            if (IsFinished) return;

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
                _rollingAccumulator++;
                return;
            }

            var currentRound = RoundResult.PlayerTwoWins;
            if (_currentPlayerOneMove == Move.Dynamite && _currentPlayerTwoMove != Move.Waterbomb)
            {
                currentRound = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Waterbomb && _currentPlayerTwoMove == Move.Dynamite)
            {
                currentRound = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerTwoMove == Move.Waterbomb && _currentPlayerOneMove != Move.Dynamite)
            {
                currentRound = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Rock && _currentPlayerTwoMove == Move.Scissors)
            {
                currentRound = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Paper && _currentPlayerTwoMove == Move.Rock)
            {
                currentRound = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Scissors && _currentPlayerTwoMove == Move.Paper)
            {
                currentRound = RoundResult.PlayerOneWins;
            }

            LastResult = currentRound;
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
            if (LastResult == RoundResult.PlayerOneWins)
            {
                PlayerOneScore++;
                PlayerOneScore += _rollingAccumulator;
                _rollingAccumulator = 0;
            }
            if (LastResult == RoundResult.PlayerTwoWins)
            {
                PlayerTwoScore++;
                PlayerTwoScore += _rollingAccumulator;
                _rollingAccumulator = 0;
            }
        }
    }
}
