namespace Game
{
    public class TournamentConfiguration
    {
        private static readonly TournamentConfiguration DefaultField = new TournamentConfiguration { DynamiteLimit = 100, NumberOfRounds = 2, TurnsPerRound = 1000 };

        public static TournamentConfiguration Default
        {
            get { return DefaultField; }
        }

        public int NumberOfRounds { get; set; }

        public int TurnsPerRound { get; set; }

        public int? DynamiteLimit { get; set; }
    }
}