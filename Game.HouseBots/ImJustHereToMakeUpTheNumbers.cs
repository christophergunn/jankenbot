namespace Game.HouseBots
{
    public class ImJustHereToMakeUpTheNumbers : IBotAi
    {
        private int _dynamiteLimit;

        public string Name { get { return "I'm just here to make up the numbers :("; } }
        
        public Move GetMove()
        {
            if (_dynamiteLimit > 0)
            {
                _dynamiteLimit--;
                return Move.Dynamite;
            }
            return Move.Waterbomb;
        }

        public void InformOfGameAgainst(TournamentPlayer opponent, int numberOfTurns, int dynamiteLimit)
        {
            _dynamiteLimit = dynamiteLimit;
        }
    }
}
