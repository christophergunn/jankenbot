using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.WebApp.Client.Configuration;
using Game.WebApp.Client.GameServerRequests;
using NUnit.Framework;

namespace Game.WebApp.Client.Tests
{
    public class IntegrationTests
    {
        private string _serverUrl;

        [SetUp]
        public void Setup()
        {
            _serverUrl = "http://localhost/Game.WebApp";
        }

        [Test]
        public void StartGameRequest_ShouldNotThrowEx()
        {
            var req = new StartGameRequest(_serverUrl);
            req.Execute();
        }

        [Test]
        public void RegisterRequest_AfterStartGame_ShouldNotThrowEx()
        {
            var req = new StartGameRequest(_serverUrl);
            req.Execute();

            string id = "123456789";
            string name = "Mr Bob";

            var req2 = new RegistrationRequest(_serverUrl, id, name);

            req2.Execute();
        }
    }
}
