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
        public const string OPPONENT_ID_KEY = "id";
        public const string OPPONENT_NAME_KEY = "name";

        private readonly Uri _playerEndpoint;
        private WebClient _webAccess;

        public OutgoingPlayerChannel(Uri playerEndpoint)
        {
            _playerEndpoint = playerEndpoint;
            _webAccess = new WebClient { BaseAddress = _playerEndpoint.ToString() };
        }

        #region IPlayerCommunicationChannel Members

        public void InformOfGameAgainst(TournamentPlayer opponent)
        {
            _webAccess.UploadValues("/inform", new NameValueCollection { { OPPONENT_ID_KEY, opponent.Id }, { OPPONENT_NAME_KEY, opponent.Name } });
        }

        public Task<Move> RequestMove()
        {
            return _webAccess.DownloadStringTaskAsync("/getmove").ContinueWith<Move>(MapResponseToMove);
        }

        private Move MapResponseToMove(Task<string> obj)
        {
            Enum.Parse(typeof(Move), (obj.Result ?? "").Trim(), true);
        }

        #endregion
    }
}