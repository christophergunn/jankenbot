namespace Game
{
    public class TournamentGame : Game
    {
        private readonly TournamentPlayer _playerOne;
        private readonly TournamentPlayer _playerTwo;

        public TournamentGame(TournamentPlayer playerOne, TournamentPlayer playerTwo)
        {
            _playerOne = playerOne;
            _playerTwo = playerTwo;
        }

        public TournamentPlayer PlayerOne { get { return _playerOne; } }

        public TournamentPlayer PlayerTwo { get { return _playerTwo; } }

        public void PlayMoves(string id1, Move move1, string id2, Move move2)
        {
            if (PlayerOne.Id == id1)
            {
                PlayMoves(move1, move2);
            }
            else
            {
                PlayMoves(move2, move1);
            }
        }
    }
}