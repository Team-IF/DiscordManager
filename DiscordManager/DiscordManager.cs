using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordManager.Command;
using DiscordManager.Config;
using DiscordManager.Event;
using DiscordManager.Interfaces;
using DiscordManager.Logging;
using DiscordManager.Service;

namespace DiscordManager
{
  /// <summary>
  ///   It's DiscordManager Core
  /// </summary>
  public class DiscordManager : Events
  {
    private readonly Game _activity;
    private readonly ConfigManager _configManager;
    private readonly ObjectService _objectService;
    private readonly int[]? _shardIds;
    private readonly UserStatus _status;
    private readonly int? _totalShard;
    public readonly string Prefix;

    internal DiscordManager(BuildOption option) : base(option.LogLevel)
    {
      Manager = this;
      _totalShard = option.Shards;
      _shardIds = option.ShardIds;
      Client = option.Client;
      _status = option.BotStatus;
      _activity = option.Game;
      Prefix = option.Prefix;
      if (Client == null)
      {
        var socketConfig = option.SocketConfig ?? new DiscordSocketConfig
          {MessageCacheSize = 100, TotalShards = _totalShard};
        if (_totalShard.HasValue)
          Client = new DiscordShardedClient(_shardIds, socketConfig);
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

      if (option.UseObjectService)
        _objectService = new ObjectService();

      if (option.CommandConfig != null)
      {
        _clientLogger.DebugAsync("Load CommandModules...");
        CommandManager.LoadCommands(Client, option.CommandConfig.HelpArg);
        Client.MessageReceived += option.CommandConfig.CommandFunc ?? ClientOnMessageReceived;
      }
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
      CommandManager.ExecuteCommand(arg, commandName);
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

      await Client.SetStatusAsync(_status);
      if (_activity != null)
        await Client.SetActivityAsync(_activity);
      await Task.Delay(-1);
    }

    private void RegisterEvents()
    {
      Client.Log += message =>
        _log.Invoke(new LogObject(LogLevel.INFO, message.Source, message.Message, message.Exception));
    }

    public void AddObject(object obj)
    {
      _objectService.Add(obj);
    }

    public void AddObject<T>(params object[] obj)
    {
      _objectService.Add<T>(obj);
    }

    public void AddObject<T>()
    {
      _objectService.Add<T>();
    }

    public T GetObject<T>()
    {
      return _objectService.Get<T>();
    }

    public void RemoveObject<T>()
    {
      _objectService.Remove<T>();
    }

    public void RemoveObject(object obj)
    {
      _objectService.Remove(obj);
    }

    public T GetConfig<T>() where T : IConfig
    {
      return _configManager.Get<T>();
    }

    public object GetConfig(object obj)
    {
      return _configManager.Get(obj);
    }

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