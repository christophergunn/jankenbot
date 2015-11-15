using System;
using System.Linq;
using Game.MatchMakers;

namespace Game.App.Console
{
    class Program
    {
        private static TournamentController _tournament;

        static void Main(string[] args)
        {
            // Setup tournament
            _tournament = new TournamentController(new SimplePartitioningMatchMaker());

            // Read tournament config values
            _tournament.Setup(ReadConfig());

            RegisterPlayers();

            // Begin rounds
            _tournament.BeginNewRound();

            foreach (var roundNum in Enumerable.Range(1, _tournament.Config.NumberOfRounds))
            {
                _tournament.PlayRound();
            }

            // End game - print scores
            PrintEndGameInfo();

            ConsoleUi.PrintExitMessageAndWait();
        }

        private static void RegisterPlayers()
        {
            ConsoleUi.WriteTextLine("Register players!" + Environment.NewLine);
            bool registerMore = true;
            while (registerMore)
            {
                ConsoleUi.WriteTextLine("Please enter player name: ");
                var name = ConsoleUi.ReadText();
                var id = Guid.NewGuid().ToString();
                var player = new TournamentPlayer(id, name);
                var consoleCommChannel = new ConsoleCommChannel(player);
                player.Comms = consoleCommChannel;
                _tournament.RegisterPlayer(player);

                ConsoleUi.WriteTextLine(string.Format("Registered player \"{0}\" with auto-ID \"{1}\".", name, id));
                char readValue = ConsoleUi.WriteTextThenReadKey("Do you want to add another player (Y/n)?");
                registerMore = (readValue == 'Y' || readValue == 'y' || readValue == (char)13);
            }
        }

        private static TournamentConfiguration ReadConfig()
        {
            ConsoleUi.WriteTextLine("Please enter number of rounds (1): ");
            var numberOfRounds = ConsoleUi.ReadInt() ?? 1;
            ConsoleUi.WriteTextLine("Please enter number of turns per round (5): ");
            var turnsPerRound = ConsoleUi.ReadInt() ?? 5;
            ConsoleUi.WriteTextLine("Please enter number of dynamites per player (1): ");
            var numberOfDynamitePerPlayer = ConsoleUi.ReadInt() ?? 1;

            return new TournamentConfiguration
            {
                DynamiteLimit = numberOfDynamitePerPlayer,
                NumberOfRounds = numberOfRounds,
                TurnsPerRound = turnsPerRound
            };
        }

        private static void PrintEndGameInfo()
        {
            PrintHeadlineScore();
            PrintBreakdown();
        }

        private static void PrintHeadlineScore()
        {
            var winner = _tournament.Players.OrderByDescending(tp => tp.Score.WonGames).FirstOrDefault();
            ConsoleUi.WriteTextLine(new string('=', 32));
            ConsoleUi.WriteTextLine("= WINNER: " + winner.Name + new string(' ', 21 - winner.Name.Length) + "=");
            ConsoleUi.WriteTextLine(new string('=', 32));
            ConsoleUi.WriteTextLine(" ");
        }

        private static void PrintBreakdown()
        {
            ConsoleUi.WriteTextLine("Breakdown:");
            foreach (var player in _tournament.Players.OrderByDescending(tp => tp.Score.WonGames))
            {
                ConsoleUi.WriteTextLine(string.Format("Player {0}: {1}/{2}/{3}", player.Name, player.Score.WonGames, player.Score.DrawnGames, player.Score.LostGames));
            }
        }
    }
}
