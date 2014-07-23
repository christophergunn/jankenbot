using System;
using Nancy.Hosting.Self;

namespace Game.WebApp.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:1234")))
            {
                host.Start();

                Console.WriteLine("Bot started, press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
