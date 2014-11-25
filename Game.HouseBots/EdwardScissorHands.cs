using Game.HouseBots.Api;

namespace Game.HouseBots
{
    public class EdwardScissorHands : IBotAi
    {
        private TournamentPlayer _currentOpponent;
        private int _currentRoundLength;
        private int _currentDynamiteLimit;

        public void InformOfGameAgainst(TournamentPlayer opponent, int numberOfTurns, int dynamiteLimit)
        {
            _currentOpponent = opponent;
            _currentRoundLength = numberOfTurns;
            _currentDynamiteLimit = dynamiteLimit;
        }

        public Move GetMove()
        {
            return Move.Scissors;
        }

        public string Name
        {
            get { return "Edward"; }
        }
    }
}
