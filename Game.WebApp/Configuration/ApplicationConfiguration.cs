using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Game.WebApp.Configuration
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private const string REG_PERIOD_MINS_KEY = "RegistrationPeriodMins";
        private const string DYNAMITE_LIMIT_KEY = "DynamiteLimit";
        private const string INTER_ROUND_BREAK = "InterRoundBreak";
        private const string NUMBER_OF_ROUNDS_KEY = "NumberOfRounds";
        private const string TURNS_PER_ROUND_KEY = "TurnsPerRound";

        private const int DefaultDynamiteLimit = 10;

        public ApplicationConfiguration()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains(DYNAMITE_LIMIT_KEY))
            {
                int dynamiteLimitTmp;
                DynamiteLimit = int.TryParse(ConfigurationManager.AppSettings[DYNAMITE_LIMIT_KEY], out dynamiteLimitTmp) ? dynamiteLimitTmp : DefaultDynamiteLimit;
            }
            else
            {
                DynamiteLimit = DefaultDynamiteLimit;
            }
            RegistrationPeriodMins = int.Parse(ConfigurationManager.AppSettings[REG_PERIOD_MINS_KEY]);
            NumberOfRounds = int.Parse(ConfigurationManager.AppSettings[NUMBER_OF_ROUNDS_KEY]);
            TurnsPerRound = int.Parse(ConfigurationManager.AppSettings[TURNS_PER_ROUND_KEY]);
        }

        public TimeSpan GetInterRoundBreak(int sequenceNumber)
        { 
            return DateTime.ParseExact(ConfigurationManager.AppSettings[INTER_ROUND_BREAK], "HH:mm:ss", CultureInfo.CurrentCulture).TimeOfDay;
        }

        public int? DynamiteLimit { get; private set; }
        public int NumberOfRounds { get; private set; }
        public int RegistrationPeriodMins { get; private set; }
        public int TurnsPerRound { get; private set; }
    }
}