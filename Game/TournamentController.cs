using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Utilities;

namespace Game
{
    public class TournamentController
    {
        private readonly IMatchMaker _matchMaker;
        private readonly Dictionary<string, TournamentPlayer> _playerIdToPlayerMap = new Dictionary<string, TournamentPlayer>();
        private readonly Dictionary<string, IPlayerCommunicationChannel> _playerIdToCommsMapping = new Dictionary<string, IPlayerCommunicationChannel>();
        private readonly List<TournamentRound> _previousAndCurrentRounds = new List<TournamentRound>();

        public TournamentController(IMatchMaker matchMaker)
        {
            _matchMaker = matchMaker;
            Config = TournamentConfiguration.Default;
        }

        public IEnumerable<TournamentPlayer> Players { get { return _playerIdToPlayerMap.Values; } }
        public TournamentRound CurrentRound { get; private set; }
        public TournamentConfiguration Config { get; private set; }

        public void Setup(TournamentConfiguration tournamentConfiguration)
        {
            Config = tournamentConfiguration;
        }

        public void RegisterPlayer(TournamentPlayer player)
        {
            _playerIdToPlayerMap[player.Id] = player;
            _playerIdToCommsMapping[player.Id] = player.Comms;
        }

        public void BeginNewRound()
        {
            StoreCurrentRound();

            if (IsFinished)
                return;

            CreateNewCurrentRound();
            CreateGamesForCurrentRound();
            InformPlayersOfOpponents();
        }

        private void StoreCurrentRound()
        {
            if (CurrentRound != null)
            {
                _previousAndCurrentRounds.Add(CurrentRound);
            }
        }

        private void CreateNewCurrentRound()
        {
            var lastRound = CurrentRound;
            CurrentRound = new TournamentRound(lastRound == null ? 1 : lastRound.SequenceNumber + 1, new List<TournamentPlayer>(Players));
        }

        private void CreateGamesForCurrentRound()
        {
            _matchMaker.Invoke(CurrentRound.Players);
            CurrentRound.SetGames(
                from match in _matchMaker.Matches
                select CreateGameBetweenOpponents(match));
        }

        public void PlayRound()
        {
            CurrentRound.Games.ForEach(PlayGame);
        }

        public void PlayGame(TournamentGame game)
        {
            while (!game.IsFinished)
            {
                var playerOneMovePromise = game.PlayerOne.Comms.RequestMove();
                var playerTwoMovePromise = game.PlayerTwo.Comms.RequestMove();

                Task.WaitAll(playerOneMovePromise, playerTwoMovePromise);

                var playerOneMove = playerOneMovePromise.Result;
                var playerTwoMove = playerTwoMovePromise.Result;

                PlayMove(game.PlayerOne, playerOneMove, game.PlayerTwo, playerTwoMove);
            }
        }


        public void PlayMove(TournamentPlayer registeredPlayer1, Move move1, TournamentPlayer registeredPlayer2, Move move2)
        {
            var game = CurrentRound.Games.FirstOrDefault(g => g.PlayerOne.Id == registeredPlayer1.Id || g.PlayerTwo.Id == registeredPlayer1.Id);
            game.RecordMoves(registeredPlayer1.Id, move1, registeredPlayer2.Id, move2);
            if (CurrentRound.IsFinished)
            {
                SetPlayerRoundScores();
                BeginNewRound();
            }
        }

        private void SetPlayerRoundScores()
        {
            CurrentRound.Games.ForEach(SetPlayerScoresFromGame);
        }

        private void SetPlayerScoresFromGame(TournamentGame game)
        {
            if (game.PlayerOneScore > game.PlayerTwoScore)
            {
                game.PlayerOne.Score.WonRounds++;
                game.PlayerTwo.Score.LostRounds++;
            }
            else if (game.PlayerTwoScore > game.PlayerOneScore)
            {
                game.PlayerOne.Score.LostRounds++;
                game.PlayerTwo.Score.WonRounds++;
            }
            else
            {
                game.PlayerOne.Score.DrawnRounds++;
                game.PlayerTwo.Score.DrawnRounds++;
            }
        }

        private TournamentGame CreateGameBetweenOpponents(Tuple<TournamentPlayer, TournamentPlayer> match)
        {
            var game = new TournamentGame(match.Item1, match.Item2);
            if (Config.DynamiteLimit.HasValue) game.SetDynamiteLimit(Config.DynamiteLimit.Value);
            game.SetRoundLimit(Config.TurnsPerRound);
            return game;
        }

        // TODO: really this should not be the responsibility of the Tournament(Controller), BeginNewRound should not call this
        // and callee should
        private void InformPlayersOfOpponents()
        {
            foreach (var match in _matchMaker.Matches)
            {
                _playerIdToCommsMapping[match.Item1.Id].InformOfGameAgainst(match.Item2);
                _playerIdToCommsMapping[match.Item2.Id].InformOfGameAgainst(match.Item1);
            }
        }

        public bool IsFinished
        {
            get
            {
                return
                    Config != null &&
                    _previousAndCurrentRounds.Count == Config.NumberOfRounds &&
                    _previousAndCurrentRounds.All(r => r.IsFinished);
            }
        }
    }
}
