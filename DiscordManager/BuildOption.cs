using System.Reflection;
using Discord;
using Discord.WebSocket;
using DiscordManager.Logging;

namespace DiscordManager
{
  internal class BuildOption
  {
    public const TokenType Type = TokenType.Bot;

    public static readonly string Version = typeof(BuildOption).Assembly
      .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public UserStatus BotStatus = UserStatus.Online;
    public BaseSocketClient? Client;
    public Game? Game;
    public LogLevel LogLevel = LogLevel.INFO;
    public string Path;
    public string Prefix;
    public int[]? ShardIds;
    public int? Shards;
    public DiscordSocketConfig? SocketConfig;
    public bool UseCommandModule = false;
    public bool UseConfig = false;
  }
}