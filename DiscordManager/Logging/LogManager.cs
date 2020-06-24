using System;
using System.Reflection;
using System.Threading.Tasks;
using DiscordManager.Event;

namespace DiscordManager.Logging
{
    internal class LogManager
    {
        private readonly Event<Func<LogObject, Task>> _messageEvent = new Event<Func<LogObject, Task>>();
        public event Func<LogObject, Task> Message
        {
            add => _messageEvent.Add(value);
            remove => _messageEvent.Remove(value);
        }

        private Logger _privateLogger { get; }
        public LogLevel LogLevel { get; }
        public LogManager(LogLevel logLevel)
        {
            LogLevel = logLevel;
            _privateLogger = new Logger(this, "Discord Manager");
        }

        private async Task LogAsync(LogLevel logLevel, string source, string message, Exception exception = null)
        {
            try
            {
                if (logLevel <= LogLevel)
                    await _messageEvent.Invoke(new LogObject(logLevel, source, message, exception)).ConfigureAwait(false);
            }
            catch
            {
                // ignored
            }
        }

        private async Task LogAsync(LogLevel logLevel, string source, Exception exception)
        {
            try
            {
                if (logLevel <= LogLevel)
                    await _messageEvent.Invoke(new LogObject(logLevel, source, null, exception)).ConfigureAwait(false);
            }
            catch
            {
                // ignored
            }
        }

        public Task InfoAsync(string source, Exception exception) => LogAsync(LogLevel.INFO, source, exception);
        public Task InfoAsync(string source, string message, Exception exception = null) =>
            LogAsync(LogLevel.INFO, source, message, exception);
        public Task ErrorAsync(string source, string message, Exception exception = null) => LogAsync(LogLevel.ERROR, source, message, exception);
        public Task DebugAsync(string source, string message, Exception exception = null) => LogAsync(LogLevel.DEBUG, source, message, exception);
        public Task CriticalAsync(string source, string message, Exception exception = null) => LogAsync(LogLevel.CRITICAL, source, message, exception);
        public Logger CreateLogger(string name) => new Logger(this, name);

        public async Task PrintVersion()
        {
            await _privateLogger.InfoAsync($"Discord Manager v{BuildOption.Version}").ConfigureAwait(false);
        }
    }
}