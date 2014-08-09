using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Game.MatchMakers;
using Game.WebApp.Api;
using Game.WebApp.Configuration;

namespace Game.WebApp.Controller
{
    public class GameController
    {
        private readonly IOutgoingPlayerChannelFactory _channelFactory;
        private readonly IApplicationConfiguration _applicationConfiguration;
        private readonly IActionScheduler _actionScheduler;

        public bool IsRunning 
        {
            get
            {
                return CurrentState == State.RegistrationPhase || CurrentState == State.InBetweenRounds ||
                       CurrentState == State.RunningRounds;
            }
        }

        public State CurrentState { get; private set; }
        public TournamentController Tournament { get; private set; }

        public enum State
        {
            NotStarted,
            RegistrationPhase,
            RunningRounds,
            InBetweenRounds,
            Complete
        }

        public GameController(
            TournamentController tournament, 
            IOutgoingPlayerChannelFactory channelFactory, 
            IApplicationConfiguration applicationConfiguration,
            IActionScheduler actionScheduler)
        {
            Tournament = tournament;
            _channelFactory = channelFactory;
            _applicationConfiguration = applicationConfiguration;
            _actionScheduler = actionScheduler;
        }

        public void Register(string id, string name, string ip)
        {
            _tournament.RegisterPlayer(
                new TournamentPlayer(id, name) { Comms = _channelFactory.CreateFromIp(ip) });
        }

        public void Start()
        {
            if (CurrentState != State.NotStarted) return;
            CurrentState = State.RegistrationPhase;

            Tournament.Setup(new TournamentConfiguration { DynamiteLimit = _applicationConfiguration.DynamiteLimit, NumberOfRounds = _applicationConfiguration.NumberOfRounds, TurnsPerRound = _applicationConfiguration.TurnsPerRound });
            RegisterBots();

            _actionScheduler.ScheduleEvent(CloseRegistrationAndBeginRounds, TimeSpan.FromMinutes(_applicationConfiguration.RegistrationPeriodMins));
        }

        private void RegisterBots()
        {
        }

        private void CloseRegistrationAndBeginRounds()
        {
            PlayRoundAndScheduleNext();
        }

        private void PlayRoundAndScheduleNext()
        {
            PlayRound();
            if (CurrentState != State.Complete)
                _actionScheduler.ScheduleEvent(PlayRoundAndScheduleNext, _applicationConfiguration.GetInterRoundBreak(Tournament.CurrentRound.SequenceNumber));
        }

        private void PlayRound()
        {
            if (Tournament.IsFinished)
                return;

            CurrentState = State.RunningRounds;
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