using System;
using System.Linq;
using Game.HouseBots;
using Game.HouseBots.Api;
using Game.WebApp.Api;
using Game.WebApp.Configuration;

namespace Game.WebApp.Controller
{
    public class GameController
    {
        private readonly IOutgoingPlayerChannelFactory _channelFactory;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IActionScheduler _actionScheduler;

        private readonly Type[] _validBotTypes = new[] { typeof(HouseBots.EdwardScissorHands), typeof(HouseBots.Randomer) };

        public bool IsRunning 
        {
            get
            {
                return CurrentState == State.RegistrationPhase || CurrentState == State.InBetweenRounds ||
                       CurrentState == State.RunningRound;
            }
        }

        public State CurrentState { get; private set; }
        public TournamentController Tournament { get; private set; }

        public enum State
        {
            NotStarted,
            RegistrationPhase,
            RunningRound,
            InBetweenRounds,
            Complete
        }

        public GameController(
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

        public void Register(string id, string name, string ip)
        {
            if (CurrentState == State.NotStarted || CurrentState == State.Complete)
                throw new InvalidOperationException("Invalid state for player registration.");

            Tournament.RegisterPlayer(
                new TournamentPlayer(id, name) { Comms = _channelFactory.CreateFromHttpEndpoint(ip) });
        }

        public void Start()
        {
            if (CurrentState != State.NotStarted) return;
            CurrentState = State.RegistrationPhase;

            Tournament.Setup(new TournamentConfiguration { DynamiteLimit = _applicationConfiguration.DynamiteLimit, NumberOfRounds = _applicationConfiguration.NumberOfRounds, TurnsPerRound = _applicationConfiguration.TurnsPerRound });
            RegisterHouseBots();

            _actionScheduler.ScheduleEvent(CloseRegistrationAndBeginRounds, TimeSpan.FromMinutes(_applicationConfiguration.RegistrationPeriodMins));
        }

        private void RegisterHouseBots()
        {
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

        private void PlayFirstRoundAndScheduleNext()
        {
            CurrentState = State.RunningRound;
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
            if (CurrentState != State.Complete)
            {
                CurrentState = State.InBetweenRounds;
                _actionScheduler.ScheduleEvent(PlayRoundAndScheduleNext,
                                               _applicationConfiguration.GetInterRoundBreak(
                                                   Tournament.CurrentRound.SequenceNumber));
            }
        }

        private void PlayRound()
        {
            if (Tournament.IsFinished)
                return;

            CurrentState = State.RunningRound;
            Tournament.BeginNewRound();
            Tournament.PlayRound();

            if (Tournament.IsFinished)
                StopGame();
        }

        private void StopGame()
        {
            if (!IsRunning) return;
            CurrentState = State.Complete;
        }
    }
}