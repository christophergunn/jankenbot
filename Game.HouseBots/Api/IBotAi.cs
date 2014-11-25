namespace Game.HouseBots.Api
{
    public interface IBotAi
    {
        string Name { get; }
        Move GetMove();
        void InformOfGameAgainst(TournamentPlayer opponent, int numberOfTurns, int dynamiteLimit);
    }
}
