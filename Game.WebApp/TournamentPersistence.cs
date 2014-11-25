using Game.MatchMakers;

namespace Game.WebApp
{
    public class TournamentPersistence : ITournamentPersistence
    {
        public TournamentController LoadTournament()
        {
            return new TournamentController(new SimplePartitioningMatchMaker());
        }

        public void SaveTournament(TournamentController tournament)
        {
        }
    }

    public interface ITournamentPersistence
    {
        TournamentController LoadTournament();
        void SaveTournament(TournamentController tournament);
    }
}