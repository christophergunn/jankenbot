using System;
using System.Collections.Generic;
using System.Linq;
using Game.Utilities;

namespace Game
{
    public class TournamentController
    {
        private readonly IMatchMaker _matchMaker;
        private readonly Dictionary<string, TournamentPlayer> _playersMap = new Dictionary<string, TournamentPlayer>();
        
        public TournamentController(IMatchMaker matchMaker)
        {
            _matchMaker = matchMaker;
            Config = TournamentConfiguration.Default;
        }

        public IEnumerable<TournamentPlayer> Players { get { return _playersMap.Values; } }
        public TournamentRound CurrentRound { get; private set; }
        public TournamentConfiguration Config { get; private set; }

        public void Setup(TournamentConfiguration tournamentConfiguration)
        {
            Config = tournamentConfiguration;
        }

        public void RegisterPlayer(TournamentPlayer player)
        {
            _playersMap[player.Id] = player;
        }

        public void BeginRound()
        {
            CreateCurrentRound();            
            _matchMaker.Invoke(CurrentRound.Players);
            CreateGamesForCurrentRound();
            InformPlayersOfOpponents();
        }

        public void PlayMove(TournamentPlayer registeredPlayer1, Move move1, TournamentPlayer registeredPlayer2, Move move2)
        {
            var game = CurrentRound.Games.FirstOrDefault(g => g.PlayerOne.Id == registeredPlayer1.Id || g.PlayerTwo.Id == registeredPlayer1.Id);
            game.PlayMoves(registeredPlayer1.Id, move1, registeredPlayer2.Id, move2);
            if (CurrentRound.Games.All(g => g.IsFinished))
            {
                SetPlayerRoundScores();
                BeginRound();
            }
        }

        private void SetPlayerRoundScores()
        {
            CurrentRound.Games.ForEach(IncrementPlayerScores);
        }

        private void IncrementPlayerScores(TournamentGame game)
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

        private void CreateGamesForCurrentRound()
        {
            CurrentRound.SetGames(
                from match in _matchMaker.Matches
                select CreateGameFromMatch(match));
        }

        private TournamentGame CreateGameFromMatch(Tuple<TournamentPlayer, TournamentPlayer> match)
        {
            var game = new TournamentGame(match.Item1, match.Item2);
            if (Config.DynamiteLimit.HasValue) game.SetDynamiteLimit(Config.DynamiteLimit.Value);
            game.SetRoundLimit(Config.TurnsPerRound);
            return game;
        }

        private void CreateCurrentRound()
        {
            var lastRound = CurrentRound;
            CurrentRound = new TournamentRound(lastRound == null ? 1 : lastRound.SequenceNumber + 1, new List<TournamentPlayer>(Players));
        }

        private void InformPlayersOfOpponents()
        {
            foreach (var match in _matchMaker.Matches)
            {
                match.Item1.Comms.Start(match.Item2);
                match.Item2.Comms.Start(match.Item1);
            }
        }
    }
}
