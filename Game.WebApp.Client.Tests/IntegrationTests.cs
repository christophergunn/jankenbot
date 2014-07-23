using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Client c = new Client("http://localhost/Game.WebApp");

            c.Register(id, name);

            Assert.That(c.IsRegistered, Is.True);
        }

    }
}
