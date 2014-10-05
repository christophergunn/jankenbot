namespace Game.HouseBots
{
    public class EdwardScissorHands : IBotAi
    {
        public Move GetMove()
        {
            return Move.Scissors;
        }

        public string Name
        {
            get { return "Edward"; }
        }
    }
}
