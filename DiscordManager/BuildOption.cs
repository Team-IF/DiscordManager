using System.Reflection;
using Discord;
using Discord.WebSocket;
using DiscordManager.Interfaces;
using DiscordManager.Logging;

namespace DiscordManager
{
  internal class BuildOption
  {
    public static readonly string Version = typeof(BuildOption).Assembly
      .GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

    public UserStatus BotStatus = UserStatus.Online;
    public BaseSocketClient? Client = null;
    public CommandConfig? CommandConfig = null;
    public Game? Game;
    public LogLevel LogLevel = LogLevel.INFO;
    public string? Path;
    public string? Prefix;
    public int[]? ShardIds;
    public int? Shards;
    public DiscordSocketConfig? SocketConfig;
    public bool UseConfig = false;
    public bool UseObjectService = false;
  }
}