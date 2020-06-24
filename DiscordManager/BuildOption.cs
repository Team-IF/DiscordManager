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

        public static string Version = "0.0.2";
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