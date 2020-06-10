using System;
using DiscordManager.Interfaces;

namespace DiscordManager
{
    public class DiscordManager
    {
        internal DiscordManager(BuilderOptions options)
        {
            Console.WriteLine(options.Token);
        }
    }
}