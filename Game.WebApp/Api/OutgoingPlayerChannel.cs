using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Game.WebApp.Api
{
    public class OutgoingPlayerChannel : IPlayerCommunicationChannel
    {
        public const string OPPONENT_ID_KEY = "opponentId";
        public const string OPPONENT_NAME_KEY = "opponentName";
        public const string NUM_OF_TURNS_KEY = "numberOfTurns";
        public const string DYNAMITE_LIMIT_KEY = "dynamiteLimit";

        private readonly Uri _playerEndpoint;
        private readonly WebClient _webAccess;

        public OutgoingPlayerChannel(Uri playerEndpoint)
        {
            _playerEndpoint = playerEndpoint;
            _webAccess = new WebClient { BaseAddress = _playerEndpoint.ToString() };
        }

        #region IPlayerCommunicationChannel Members

        public void InformOfGameAgainst(TournamentPlayer opponent, int numberOfTurns, int dynamiteLimit)
        {
            _webAccess.UploadValues("/start", new NameValueCollection { { OPPONENT_ID_KEY, opponent.Id }, { OPPONENT_NAME_KEY, opponent.Name }, { NUM_OF_TURNS_KEY, numberOfTurns.ToString() }, { DYNAMITE_LIMIT_KEY, dynamiteLimit.ToString() } });
        }

        public Task<Move> RequestMove()
        {
            return _webAccess.DownloadStringTaskAsync("/move").ContinueWith<Move>(MapResponseToMove);
        }

        private Move MapResponseToMove(Task<string> obj)
        {
            return (Move)Enum.Parse(typeof(Move), (obj.Result ?? "").Trim(), true);
        }

        #endregion
    }
}