using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.WebApp.Client.Domain
{
    // I could have referenced Game project and used Game.Move enum, however I am trying 
    // to replicate a client who does not have / need the Game domain src code
    public enum ClientMove
    {
        Rock,
        Paper,
        Scissors,
        Dynamite,
        Waterbomb
    }

    public class Client
    {
        private readonly string _id;
        private readonly string _name;
        private readonly ClientLogic _clientLogic;

        public string Id { get { return _id; } }
        public string Name { get { return _name; } }

        public Client(string id, string name)
        {
            _id = id;
            _name = name;
            _clientLogic = new ClientLogic();
        }

        public void InformOfRoundStarting(string opponentId, string opponentName, int pointsToWin, int maxRounds, int dynamiteAllowance)
        {
            _clientLogic.InformOfRoundStarting(opponentId, opponentName, pointsToWin, maxRounds, dynamiteAllowance);
        }

        public ClientMove GetNextMove()
        {
            return _clientLogic.GetNextMove();
        }

        public void InformOfLastOpponentMove(ClientMove parsedMove)
        {
            _clientLogic.InformOfLastOpponentMove(parsedMove);
        }
    }
}
