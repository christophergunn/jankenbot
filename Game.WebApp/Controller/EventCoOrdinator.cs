using System;
using System.Linq;
using Game.HouseBots;
using Game.HouseBots.Api;
using Game.WebApp.Api;
using Game.WebApp.Configuration;

namespace Game.WebApp.Controller
{
    public class EventCoOrdinator
    {
        private readonly IOutgoingPlayerChannelFactory _channelFactory;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IActionScheduler _actionScheduler;

        private readonly Type[] _validBotTypes = { typeof(HouseBots.EdwardScissorHands), typeof(HouseBots.Randomer) };

        public enum EventState
        {
            BeforeStarted,
            RegistrationPhase,
            RunningRound,
            InBetweenRounds,
            Complete
        }

        public EventState CurrentState { get; private set; }

        public bool IsRunning 
        {
            get
            {
                return CurrentState == EventState.RegistrationPhase || 
                       CurrentState == EventState.InBetweenRounds ||
                       CurrentState == EventState.RunningRound;
            }
        }

        public ITournament Tournament { get; private set; }

        public EventCoOrdinator(
            ITournamentPersistence tournamentPersistence, 
            IOutgoingPlayerChannelFactory channelFactory, 
            IApplicationConfiguration applicationConfiguration,
            IActionScheduler actionScheduler)
        {
            Tournament = tournamentPersistence.LoadTournament();
            _channelFactory = channelFactory;
            _applicationConfiguration = applicationConfiguration;
            _actionScheduler = actionScheduler;
        }

        public void Start()
        {
            if (CurrentState != EventState.BeforeStarted) 
                throw new InvalidOperationException("Event can only be started once.");

            CurrentState = EventState.RegistrationPhase;

            Tournament.Setup(new TournamentConfiguration { DynamiteLimit = _applicationConfiguration.DynamiteLimit, NumberOfRounds = _applicationConfiguration.NumberOfRounds, TurnsPerRound = _applicationConfiguration.TurnsPerRound });
            RegisterHouseBots();

            _actionScheduler.ScheduleEvent(CloseRegistrationAndBeginRounds, TimeSpan.FromMinutes(_applicationConfiguration.RegistrationPeriodMins));
        }

        private void RegisterHouseBots()
        {
            // TODO: bots should be created and registered in a separate component
            foreach (var validBotType in _validBotTypes)
            {
                RegisterBot(validBotType);
            }
        }

        private void RegisterBot(Type validBotType)
        {
            var botAi = (IBotAi)Activator.CreateInstance(validBotType);
            Tournament.RegisterPlayer(new TournamentPlayer(Guid.NewGuid().ToString(), botAi.Name) { Comms = _channelFactory.CreateForBot(botAi) });
        }

        private void CloseRegistrationAndBeginRounds()
        {
            PlayFirstRoundAndScheduleNext();
        }

        public void Register(string id, string name, string ip)
        {
            if (CurrentState == EventState.BeforeStarted || CurrentState == EventState.Complete)
                throw new InvalidOperationException("Invalid event state for player registration.");

            Tournament.RegisterPlayer(
                new TournamentPlayer(id, name) { Comms = _channelFactory.CreateFromHttpEndpoint(ip) });
        }

        private void PlayFirstRoundAndScheduleNext()
        {
            CurrentState = EventState.RunningRound;
            BalancePlayerCountWithAdditionalHouseBots();
            PlayRoundAndScheduleNext();
        }

        private void BalancePlayerCountWithAdditionalHouseBots()
        {
            if (Tournament.Players.Count() % 2 != 0)
            {
                RegisterBot(typeof(ImJustHereToMakeUpTheNumbers));
            }
        }

        private void PlayRoundAndScheduleNext()
        {
            PlayRound();
            if (CurrentState != EventState.Complete)
            {
                CurrentState = EventState.InBetweenRounds;
                _actionScheduler.ScheduleEvent(PlayRoundAndScheduleNext,
                                               _applicationConfiguration.GetInterRoundBreak(
                                                   Tournament.CurrentRound.SequenceNumber));
            }
        }

        private void PlayRound()
        {
            if (Tournament.IsFinished)
                return;

            CurrentState = EventState.RunningRound;
            Tournament.BeginNewRound();
            Tournament.PlayRound();

            if (Tournament.IsFinished)
                StopGame();
        }

        private void StopGame()
        {
            if (!IsRunning) return;
            CurrentState = EventState.Complete;
        }
    }
}