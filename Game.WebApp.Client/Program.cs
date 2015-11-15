using System;
using Game.WebApp.Client.Configuration;
using Nancy.Hosting.Self;

namespace Game.WebApp.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NancyClientHostHelper.Start(() =>
            {
                var config = AppConfigReader.RetrieveClientConfig();

                Console.WriteLine("Registering bot with server '{0}'...", config.ServerUrl);

                Console.WriteLine("... done.");

                Console.WriteLine("Bot started, press any key to exit...");
                Console.ReadKey();
            });
        }
    }

    public static class NancyClientHostHelper
    {
        public static void Start(Action onStarted)
        {
            using (var host = new NancyHost(new Uri("http://localhost:1234")))
            {
                host.Start();

                onStarted();
            }
        }
    }
}
