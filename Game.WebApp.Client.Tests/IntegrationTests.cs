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

            var c = new ClientHttpInterface(new ClientConfig("http://localhost/Game.WebApp", id, name));

            c.Register();

            Assert.That(c.IsRegistered, Is.True);
        }

    }
}
