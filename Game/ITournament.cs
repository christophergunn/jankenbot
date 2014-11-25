using System.Collections.Generic;

namespace Game
{
    public interface ITournament
    {
        IEnumerable<TournamentPlayer> Players { get; }
        TournamentRound CurrentRound { get; }
        TournamentConfiguration Config { get; }

        void Setup(TournamentConfiguration tournamentConfiguration);
        void RegisterPlayer(TournamentPlayer player);
        void BeginNewRound();

        void PlayRound();
        void PlayGame(TournamentGame game);
        void PlayMove(TournamentPlayer registeredPlayer1, Move move1, TournamentPlayer registeredPlayer2,
                      Move move2);
        bool IsFinished { get; }
    }
}