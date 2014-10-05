using System.Threading.Tasks;

namespace Game
{
    public interface IPlayerCommunicationChannel
    {
        void InformOfGameAgainst(TournamentPlayer opponent, int numberOfTurns, int dynamiteLimit);

        Task<Move> RequestMove();
    }
}