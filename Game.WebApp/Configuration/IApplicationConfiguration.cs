using System;

namespace Game.WebApp.Configuration
{
    public interface IApplicationConfiguration
    {
        TimeSpan GetInterRoundBreak(int sequenceNumber);
        int? DynamiteLimit { get; set; }
        int NumberOfRounds { get; set; }
        int RegistrationPeriodMins { get; set; }
        int TurnsPerRound { get; set; }
    }
}