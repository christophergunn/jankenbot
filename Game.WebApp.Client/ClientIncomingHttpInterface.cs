using System;
using Game.WebApp.Client.Domain;
using Nancy;

namespace Game.WebApp.Client
{
    public class ClientIncomingHttpInterface : NancyModule
    {
        private readonly Domain.Client _client;
        private readonly string _id;
        private readonly string _name;

        public ClientIncomingHttpInterface(Domain.Client client)
        {
            _client = client;

            Get["/hello"] =  _ => string.Format("Hi there, my name is '{0}' and my ID is '{1}'.", _client.Name, _client.Id);

            Post["/start"] =
                o =>
                {
                    ProcessStart(o.opponentId, o.opponentName, o.pointsToWin, o.numberOfTurns, o.dynamiteLimit);
                    return HttpStatusCode.OK;
                };

            Get["/move"] = _ => GetNextMove();

            Post["/move"] = 
                o => 
                { 
                    ProcessLastOpponentMove(o.move);
                    return HttpStatusCode.OK;
                };
        }

        private void ProcessStart(string opponentId, string opponentName, string pointsToWin, string maxRounds, string dynamiteAllowance)
        {
            _client.InformOfRoundStarting(opponentId, opponentName, int.Parse(pointsToWin), int.Parse(maxRounds), int.Parse(dynamiteAllowance));
        }

        private string GetNextMove()
        {
            return _client.GetNextMove().ToString();
        }

        private void ProcessLastOpponentMove(string move)
        {
            ClientMove parsedMove;
            if (Enum.TryParse(move, out parsedMove))
            {
                _client.InformOfLastOpponentMove(parsedMove);
            }
        }
    }
}
