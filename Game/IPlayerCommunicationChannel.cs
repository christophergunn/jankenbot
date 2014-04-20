using System;
using System.Threading.Tasks;

namespace Game
{
    public interface IPlayerCommunicationChannel
    {
        void SetPlayer(TournamentPlayer p1);

        void InformOfGameAgainst(TournamentPlayer p1);

        Task<Move> RequestMove();
    }
}
