using System;
using System.Collections.Generic;

namespace Game
{
    public interface IMatchMaker
    {
        void Invoke(IEnumerable<TournamentPlayer> players);
        IList<Tuple<TournamentPlayer, TournamentPlayer>> Matches { get; } 
    }
}