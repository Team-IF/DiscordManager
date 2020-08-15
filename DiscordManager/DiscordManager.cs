using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordManager.Command;
using DiscordManager.Event;
using DiscordManager.Logging;

namespace DiscordManager
{
  /// <summary>
  ///   It's DiscordManager Core
  /// </summary>
  public class DiscordManager : Events
  {
    internal static DiscordManager Manager { get; private set; }
    private readonly Game Activity;
    private readonly string Prefix;
    private readonly int[]? ShardIds;
    private readonly UserStatus Status;
    private readonly TokenType TokenType;
    private readonly int? TotalShard;
    public BaseSocketClient Client { get; }

    internal DiscordManager(BuildOption option) : base(option.LogLevel)
    {
      TokenType = option.Type;
      TotalShard = option.Shards;
      ShardIds = option.ShardIds;
      Client = option.Client;
      Status = option.BotStatus;
      Activity = option.Game;
      Prefix = option.Prefix;
      if (Client == null)
      {
        var socketConfig = option.SocketConfig ?? new DiscordSocketConfig
          {MessageCacheSize = 100, TotalShards = TotalShard};
        if (TotalShard.HasValue)
          Client = new DiscordShardedClient(ShardIds, socketConfig);
        else
          Client = new DiscordSocketClient(socketConfig);
      }

      if (option.UseCommandModule)
      {
        _clientLogger.DebugAsync("Load CommandModules...");
        CommandManager.LoadCommands(Client);
        Client.MessageReceived += Command ?? ClientOnMessageReceived;
      }

      Manager = this;
    }


    private async Task ClientOnMessageReceived(SocketMessage arg)
    {
      if (arg.Author.IsBot || arg.Author.IsWebhook)
        return;

      var content = arg.Content.Trim();
      var splitContent = content.Split(' ');

      var firstWord = splitContent[0];
      if (!firstWord.StartsWith(Prefix)) return;
      var commandName = firstWord.Substring(Prefix.Length);
      CommandManager.ExecuteCommand(arg, commandName, new object[] {splitContent});
    }

    private async Task Init(string token)
    {
      await _clientLogger.InfoAsync("Discord Manager Initialize....").ConfigureAwait(false);
      await LogManager.PrintVersion().ConfigureAwait(false);
      await _clientLogger.DebugAsync("Check Internet is Available").ConfigureAwait(false);
      if (!NetworkInterface.GetIsNetworkAvailable())
        throw new ManagerException(
          "UnAvailable Internet Check Your Pc/Server Internet State");

      await _clientLogger.DebugAsync("Check Token is Validated").ConfigureAwait(false);
      try
      {
        TokenUtils.ValidateToken(TokenType, token);
      }
      catch (Exception e)
      {
        throw new ManagerException(
          "Token is Invalid. The token must be Validated");
      }

      await _clientLogger.DebugAsync("Successfully Check Token").ConfigureAwait(false);
      await _clientLogger.DebugAsync("Register Events...").ConfigureAwait(false);
      RegisterEvents();
      await _clientLogger.DebugAsync("Successfully Register Events").ConfigureAwait(false);
      await Client.LoginAsync(TokenType, token).ConfigureAwait(false);
      await Client.StartAsync().ConfigureAwait(false);

      await Client.SetStatusAsync(Status);
      if (Activity != null)
        await Client.SetActivityAsync(Activity);
      await Task.Delay(-1);
    }

    private void RegisterEvents()
    {
      Client.Log += message =>
        _log.Invoke(new LogObject(LogLevel.INFO, message.Source, message.Message, message.Exception));
    }

    public void Run(string token)
    {
      try
      {
        Init(token).GetAwaiter().GetResult();
      }
      catch (ManagerException e)
      {
        _clientLogger.CriticalAsync(e.Message, e).ConfigureAwait(false);
      }
    }
  }
}