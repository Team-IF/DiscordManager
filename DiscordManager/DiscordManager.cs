using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordManager.Command;
using DiscordManager.Config;
using DiscordManager.Event;
using DiscordManager.Logging;
using DiscordManager.Service;

namespace DiscordManager
{
  /// <summary>
  ///   It's DiscordManager Core
  /// </summary>
  public class DiscordManager : Events
  {
    private readonly Game Activity;
    private readonly ConfigManager _configManager;
    private readonly int[]? ShardIds;
    private readonly UserStatus Status;
    private readonly int? TotalShard;
    private readonly ObjectService _objectService;
    public readonly string Prefix;

    internal DiscordManager(BuildOption option) : base(option.LogLevel)
    {
      Manager = this;
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

      if (option.UseConfig)
      {
        LoaderConfig.Path = option.Path;
        _configManager = new ConfigManager();
        _configManager.Load().ConfigureAwait(false);
        var config = GetConfig<Common>();
        Prefix = config.Prefix;
      }

      if (option.UseCommandModule)
      {
        _clientLogger.DebugAsync("Load CommandModules...");
        CommandManager.LoadCommands(Client);
        Client.MessageReceived += Command ?? ClientOnMessageReceived;
      }
      
      if (option.UseObjectService)
        _objectService = new ObjectService();
    }

    internal static DiscordManager Manager { get; private set; }
    public BaseSocketClient Client { get; }

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

    private async Task Init(string token, TokenType type)
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
        TokenUtils.ValidateToken(type, token);
      }
      catch (Exception e)
      {
        throw new ManagerException(
          "Token is Invalid. The token must be Validated", e);
      }

      await _clientLogger.DebugAsync("Successfully Check Token").ConfigureAwait(false);
      await _clientLogger.DebugAsync("Register Events...").ConfigureAwait(false);
      RegisterEvents();
      await _clientLogger.DebugAsync("Successfully Register Events").ConfigureAwait(false);
      await Client.LoginAsync(type, token).ConfigureAwait(false);
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

    public void AddObject<T>(params object[] obj)
    {
      _objectService.Add<T>(obj);
    }
    public T GetObject<T>() => _objectService.Get<T>();

    public T GetConfig<T>() where T : IConfig => _configManager.Get<T>();
    public object GetConfig(object obj) => _configManager.Get(obj);

    public void Run(string token = null, TokenType type = TokenType.Bot)
    {
      try
      {
        Init(token ?? _configManager.Get<Common>().Token, type).GetAwaiter().GetResult();
      }
      catch (ManagerException e)
      {
        _clientLogger.CriticalAsync(e.Message, e).ConfigureAwait(false);
      }
    }
  }
}