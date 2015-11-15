using System.Runtime.Remoting.Messaging;

namespace Game
{
    public enum RoundResult
    {
        PlayerOneWins,
        NeitherWon,
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

    public class GameConfig
    {
        private readonly int _roundLimit;
        private readonly int? _dynamiteLimit;
        
        public int RoundLimit { get { return _roundLimit; } }
        public int? DynamiteLimit { get { return _dynamiteLimit; }}

        public GameConfig(int roundLimit, int? dynamiteLimit = null)
        {
            _roundLimit = roundLimit;
            _dynamiteLimit = dynamiteLimit;
        }
    }

    public class Game
    {
        private int _roundLimit;
        private int _roundsPlayed;
        private int? _dynamiteLimit;
        private Move _currentPlayerOneMove;
        private Move _currentPlayerTwoMove;
        private int _rollingScoreAccumulator;

        public int PlayerOneScore { get; private set; }
        public int PlayerOneRemainingDynamite { get; private set; }

        public int PlayerTwoScore { get; private set; }
        public int PlayerTwoRemainingDynamite { get; private set; }

        public RoundResult? LastResult { get; private set; }

        public RoundResult? FinalState { get; private set; }
        public bool IsFinished 
        {
            get { return _roundsPlayed == _roundLimit; }
        }

        public void SetRoundLimit(int turns)
        {
            _roundLimit = turns;
        }

        public void SetDynamiteLimit(int limit)
        {
            _dynamiteLimit = limit;
            PlayerOneRemainingDynamite = limit;
            PlayerTwoRemainingDynamite = limit;
        }

        public void PlayMoves(Move playerOneMove, Move playerTwoMove)
        {
            if (IsFinished) 
                return;

            _currentPlayerOneMove = playerOneMove;
            _currentPlayerTwoMove = playerTwoMove;

            ApplyDynamiteStockLimit();
            DetermineRoundWinner();
            RecordRoundWinner();

            _roundsPlayed++;
            if (IsFinished)
                ProcessGameFinished();
        }

        private void DetermineRoundWinner()
        {
            if (_currentPlayerOneMove == _currentPlayerTwoMove)
            {
                LastResult = RoundResult.NeitherWon;
                _rollingScoreAccumulator++;
                return;
            }

            var roundResult = RoundResult.PlayerTwoWins;
            if (_currentPlayerOneMove == Move.Dynamite && _currentPlayerTwoMove != Move.Waterbomb)
            {
                roundResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Waterbomb && _currentPlayerTwoMove == Move.Dynamite)
            {
                roundResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerTwoMove == Move.Waterbomb && _currentPlayerOneMove != Move.Dynamite)
            {
                roundResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Rock && _currentPlayerTwoMove == Move.Scissors)
            {
                roundResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Paper && _currentPlayerTwoMove == Move.Rock)
            {
                roundResult = RoundResult.PlayerOneWins;
            }
            else if (_currentPlayerOneMove == Move.Scissors && _currentPlayerTwoMove == Move.Paper)
            {
                roundResult = RoundResult.PlayerOneWins;
            }

            LastResult = roundResult;
        }

        private void ApplyDynamiteStockLimit()
        {
            if (!_dynamiteLimit.HasValue) 
                return;

            if (_currentPlayerOneMove == Move.Dynamite && PlayerOneRemainingDynamite == 0)
            {
                _currentPlayerOneMove = Move.Waterbomb;
            }
            if (_currentPlayerTwoMove == Move.Dynamite && PlayerTwoRemainingDynamite == 0)
            {
                _currentPlayerTwoMove = Move.Waterbomb;
            }

            ReduceDynamiteStock();
        }

        private void ReduceDynamiteStock()
        {
            if (_currentPlayerOneMove == Move.Dynamite)
            {
                if (PlayerOneRemainingDynamite > 0)
                    PlayerOneRemainingDynamite--;
            }
            if (_currentPlayerTwoMove == Move.Dynamite)
            {
                if (PlayerTwoRemainingDynamite > 0)
                    PlayerTwoRemainingDynamite--;
            }
        }

        private void ProcessGameFinished()
        {
            if (_roundsPlayed == _roundLimit)
            {
                if (PlayerOneScore == PlayerTwoScore) FinalState = RoundResult.NeitherWon;
                else if (PlayerOneScore > PlayerTwoScore) FinalState = RoundResult.PlayerOneWins;
                else FinalState = RoundResult.PlayerTwoWins;
            }
        }

        private void RecordRoundWinner()
        {
            if (LastResult == RoundResult.PlayerOneWins)
            {
                PlayerOneScore++;
                PlayerOneScore += _rollingScoreAccumulator;
                _rollingScoreAccumulator = 0;
            }

            if (LastResult == RoundResult.PlayerTwoWins)
            {
                PlayerTwoScore++;
                PlayerTwoScore += _rollingScoreAccumulator;
                _rollingScoreAccumulator = 0;
            }
        }
    }
}
