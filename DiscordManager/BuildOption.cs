using System.Reflection;
using Discord;
using Discord.WebSocket;
using DiscordManager.Logging;

namespace DiscordManager
{
    internal class BuildOption
    {
        internal BuildOption()
        {
        }

        public static readonly string Version = typeof(BuildOption).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        public BaseSocketClient? Client;
        public DiscordSocketConfig? SocketConfig;
        public TokenType Type = TokenType.Bot;
        public UserStatus BotStatus = UserStatus.Online;
        public Game? Game;
        public LogLevel LogLevel = LogLevel.INFO;
        public int? Shards;
        public int[]? ShardIds;
        public bool UseCommandModule = false;
        public string Prefix;
    }
}