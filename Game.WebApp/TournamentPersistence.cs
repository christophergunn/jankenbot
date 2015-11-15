using Game.MatchMakers;

namespace Game.WebApp
{
    public class TournamentPersistence : ITournamentPersistence
    {
        public ITournament LoadTournament()
        {
            return new TournamentController(new SimplePartitioningMatchMaker());
        }

        public void SaveTournament(ITournament tournament)
        {
        }
    }

    public interface ITournamentPersistence
    {
        ITournament LoadTournament();
        void SaveTournament(ITournament tournament);
    }
}