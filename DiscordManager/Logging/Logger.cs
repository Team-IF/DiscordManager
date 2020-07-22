using System;
using System.Threading.Tasks;

namespace DiscordManager.Logging
{
  internal class Logger
  {
    private readonly LogManager _logManager;

    public Logger(LogManager logManager, string name)
    {
      _logManager = logManager;
      Name = name;
    }

    public string Name { get; }
    public LogLevel Level => _logManager.LogLevel;


    public Task InfoAsync(string message, Exception exception = null)
    {
      return _logManager.InfoAsync(Name, message, exception);
    }

    public Task InfoAsync(Exception exception)
    {
      return _logManager.InfoAsync(Name, exception);
    }

    public Task ErrorAsync(string message, Exception exception = null)
    {
      return _logManager.ErrorAsync(Name, message, exception);
    }

    public Task DebugAsync(string message, Exception exception = null)
    {
      return _logManager.DebugAsync(Name, message, exception);
    }

    public Task CriticalAsync(string message, Exception exception = null)
    {
      return _logManager.CriticalAsync(Name, message, exception);
    }
  }
}