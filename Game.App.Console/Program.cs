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

            // End game
        }

        private static void RegisterPlayers()
        {
            WriteText("Register players!");
            bool registerMore = true;
            while (registerMore)
            {
                WriteText("Please enter player name: ");
                var name = ReadText();
                var id = Guid.NewGuid().ToString();
                _tournament.RegisterPlayer(new TournamentPlayer(id, name, new ConsoleCommChannel()));

                WriteText(string.Format("Registered player \"{0}\" with ID \"{1}\".", name, id));
                char readValue = WriteTextThenReadKey("Do you want to add another player (Y/n)?");
                registerMore = (readValue == 'Y' || readValue == 'y');
            }
        }

        private static char WriteTextThenReadKey(string text)
        {
            WriteText(text);
            return System.Console.ReadKey().KeyChar;
        }

        private static TournamentConfiguration ReadConfig()
        {
            WriteText("Please enter number of rounds (1): ");
            var numberOfRounds = ReadInt() ?? 1;
            WriteText("Please enter number of turns per round (5): ");
            var turnsPerRound = ReadInt() ?? 5;
            WriteText("Please enter number of dynamites per player (1): ");
            var numberOfDynamitePerPlayer = ReadInt() ?? 1;

            return new TournamentConfiguration
                {
                    DynamiteLimit = numberOfDynamitePerPlayer,
                    NumberOfRounds = numberOfRounds,
                    TurnsPerRound = turnsPerRound
                };
        }

        private static int? ReadInt()
        {
            var text = System.Console.In.ReadLine();
            if (string.IsNullOrWhiteSpace(text))
                return null;
            return Int32.Parse(text);
        }

        private static string ReadText()
        {
            return System.Console.In.ReadLine();
        }

        private static void WriteText(string text)
        {
            System.Console.Out.WriteLine(text);
        }
    }
}
