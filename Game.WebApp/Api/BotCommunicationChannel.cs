﻿using System;
using System.Threading.Tasks;
using Game.HouseBots;

namespace Game.WebApp.Api
{
    public class BotCommunicationChannel : IPlayerCommunicationChannel
    {
        private readonly IBotAi _bot;

        public BotCommunicationChannel(IBotAi bot)
        {
            _bot = bot;
        }

        public void InformOfGameAgainst(TournamentPlayer opponent, int numberOfTurns, int dynamiteLimit)
        {
            _bot.InformOfGameAgainst(opponent, numberOfTurns, dynamiteLimit);
        }

        public Task<Move> RequestMove()
        {
            return Task.FromResult(_bot.GetMove());
        }
    }
}