using System;

namespace Game.App.Console
{
    public static class ConsoleUi
    {
        public static char WriteTextThenReadKey(string text)
        {
            WriteTextLine(text);
            return System.Console.ReadKey().KeyChar;
        }

        public static int? ReadInt()
        {
            var text = System.Console.In.ReadLine();
            if (string.IsNullOrWhiteSpace(text))
                return null;
            return Int32.Parse(text);
        }

        public static void PrintExitMessageAndWait()
        {
            WriteTextLine("Press any key to exit...");
            System.Console.ReadKey(true);
        }

        public static string ReadText()
        {
            return System.Console.In.ReadLine();
        }

        public static void WriteTextLine(string text)
        {
            System.Console.Out.WriteLine(text);
        }
    }
}