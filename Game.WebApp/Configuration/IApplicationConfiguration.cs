using System;

namespace Game.WebApp.Configuration
{
    public interface IApplicationConfiguration
    {
        TimeSpan GetInterRoundBreak(int sequenceNumber);
        int? DynamiteLimit { get; }
        int NumberOfRounds { get; }
        int RegistrationPeriodMins { get; }
        int TurnsPerRound { get; }
    }
}