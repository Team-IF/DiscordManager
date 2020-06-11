using System;

namespace DiscordManager
{
    public class DiscordManagerException : Exception
    {
        public DiscordManagerException(string message) : base(message)
        {
        }

        public DiscordManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}