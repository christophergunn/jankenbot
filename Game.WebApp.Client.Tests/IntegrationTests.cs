using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.WebApp.Client.Configuration;
using NUnit.Framework;

namespace Game.WebApp.Client.Tests
{
    public class IntegrationTests
    {
        [Test]
        public void Register_ShouldGetHttp200Back()
        {
            string id = "123456789";
            string name = "Mr Bob";

            Client c = new Client(new ClientConfig { ServerUrl = "http://localhost/Game.WebApp", Id = id, Name = name });

            c.Register();

            Assert.That(c.IsRegistered, Is.True);
        }

    }
}
