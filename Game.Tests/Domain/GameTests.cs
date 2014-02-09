using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Game.Tests.Domain
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void CanCreateGame()
        {
            new Game();
        }
    }
}
