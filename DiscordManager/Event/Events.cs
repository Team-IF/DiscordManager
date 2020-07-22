using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordManager.Command;
using DiscordManager.Logging;

namespace DiscordManager.Event
{
  public abstract class Events
  {
    public static Func<SocketMessage, Permission> Permission;
    internal readonly Logger _clientLogger;

    internal readonly Event<Func<LogObject, Task>> _log = new Event<Func<LogObject, Task>>();
    internal readonly LogManager LogManager;

    public Func<SocketMessage, Task> Command;

    internal Events(LogLevel level)
    {
      LogManager = new LogManager(level);
      LogManager.Message += async msg => await _log.Invoke(msg).ConfigureAwait(false);
      _clientLogger = LogManager.CreateLogger("Discord Manager (DM)");
      CommandManager._commandLogger = LogManager.CreateLogger("Command Manager (CM)");
    }

    public event Func<LogObject, Task> Log
    {
      add => _log.Add(value);
      remove => _log.Remove(value);
    }
  }
}