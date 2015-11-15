using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.WebApp.Client.Domain
{
    public class ClientLogic
    {
        public ClientMove GetNextMove()
        {
            return ClientMove.Dynamite;
        }

        public void InformOfRoundStarting(string opponentId, string opponentName, int pointsToWin, int maxRounds, int dynamiteAllowance)
        {

        }

        public void InformOfLastOpponentMove(ClientMove parsedMove)
        {
            // we aren't interested in what our opponent did - we're not going to learn - for now... 
        }
    }
}
