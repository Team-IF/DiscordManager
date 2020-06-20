using System;
using System.Reflection;
using System.Threading.Tasks;
using DiscordManager.Event;

namespace DiscordManager.Logging
{
    internal class Logger
    {
        private readonly LogManager _logManager;
        public string Name { get; }
        public LogLevel Level => _logManager.LogLevel;

        public Logger(LogManager logManager, string name)
        {
            _logManager = logManager;
            Name = name;
        }

        
        public Task InfoAsync(string message, Exception exception = null)
            => _logManager.InfoAsync(Name, message, exception);
        public Task InfoAsync(Exception exception) => _logManager.InfoAsync(Name, exception);

        public Task ErrorAsync(string message, Exception exception = null) => _logManager.ErrorAsync(Name, message, exception);
        public Task DebugAsync(string message, Exception exception = null) => _logManager.DebugAsync(Name, message, exception);
        public Task CriticalAsync(string message, Exception exception = null) => _logManager.CriticalAsync(Name, message, exception);
    }
}