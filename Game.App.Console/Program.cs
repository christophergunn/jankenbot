using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            // End game
        }

        private static void RegisterPlayers()
        {
            ConsoleUi.WriteText("Register players!");
            bool registerMore = true;
            while (registerMore)
            {
                ConsoleUi.WriteText("Please enter player name: ");
                var name = ConsoleUi.ReadText();
                var id = Guid.NewGuid().ToString();
                var consoleCommChannel = new ConsoleCommChannel();
                _tournament.RegisterPlayer(new TournamentPlayer(id, name, consoleCommChannel));


                ConsoleUi.WriteText(string.Format("Registered player \"{0}\" with auto-ID \"{1}\".", name, id));
                char readValue = ConsoleUi.WriteTextThenReadKey("Do you want to add another player (Y/n)?");
                registerMore = (readValue == 'Y' || readValue == 'y');
            }
        }

        private static TournamentConfiguration ReadConfig()
        {
            ConsoleUi.WriteText("Please enter number of rounds (1): ");
            var numberOfRounds = ConsoleUi.ReadInt() ?? 1;
            ConsoleUi.WriteText("Please enter number of turns per round (5): ");
            var turnsPerRound = ConsoleUi.ReadInt() ?? 5;
            ConsoleUi.WriteText("Please enter number of dynamites per player (1): ");
            var numberOfDynamitePerPlayer = ConsoleUi.ReadInt() ?? 1;

            return new TournamentConfiguration
            {
                DynamiteLimit = numberOfDynamitePerPlayer,
                NumberOfRounds = numberOfRounds,
                TurnsPerRound = turnsPerRound
            };
        }
    }
}
