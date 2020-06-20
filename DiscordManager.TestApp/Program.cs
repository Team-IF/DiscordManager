using System;
using System.Threading.Tasks;
using Discord;
using DiscordManager.Logging;

namespace DiscordManager.TestApp
{
    class Program
    {
        static void Main()
        {
            var discordManager = DiscordBuilder
                .SocketBuilder
                .WithActivity(new Game("Live For Test"))
                .WithLogLevel(LogLevel.ALL)
                .WithCommandModule()
                .Build();

            discordManager.Log += Log;
            discordManager.Run("");
        }

        public static Task Log(LogObject logObject)
        {
            Console.WriteLine(logObject.ToString());
            return Task.CompletedTask;
        }
    }
}