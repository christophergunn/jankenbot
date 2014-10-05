using Game.MatchMakers;

namespace Game.WebApp
{
    public class TournamentPersistence : ITournamentPersistence
    {
        public TournamentController GetController()
        {
            return new TournamentController(new SimplePartitioningMatchMaker());
        }
    }

    public interface ITournamentPersistence
    {
        TournamentController GetController();
    }
}