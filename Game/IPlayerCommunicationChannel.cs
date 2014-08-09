using System;
using System.Threading.Tasks;

namespace Game
{
    public interface IPlayerCommunicationChannel
    {
        void InformOfGameAgainst(TournamentPlayer opponent);

        Task<Move> RequestMove();
    }
}