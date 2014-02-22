using System.Collections.Generic;

namespace Game
{
    public interface IMatchMaker
    {
        void Invoke(IEnumerable<TournamentPlayer> players);
    }
}