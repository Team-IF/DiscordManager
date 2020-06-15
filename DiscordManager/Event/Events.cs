using System;
using System.Threading.Tasks;
using DiscordManager.Logging;

namespace DiscordManager.Event
{
    public abstract class Events
    {
        internal Events()
        {
        }

        private readonly Event<Func<LogObject, Task>> _log = new Event<Func<LogObject, Task>>();

        public event Func<LogObject, Task> Log
        {
            add => _log.Add(value);
            remove => _log.Remove(value);
        }
    }
}