using Discord.WebSocket;

namespace DiscordManager.Command
{
    public class Context
    {
        protected SocketMessage Message { get; private set; }
        /// <summary>
        /// Get Message Author
        /// </summary>
        protected SocketUser Author => Message.Author;
        /// <summary>
        /// Get Channel from Message
        /// </summary>
        protected ISocketMessageChannel Channel => Message.Channel;
        /// <summary>
        /// If message from guild Not Null
        /// Opposition is guild not null
        /// </summary>
        protected SocketGuild? Guild => (Channel as SocketGuildChannel)?.Guild;

        internal void SetMessage(SocketMessage message)
        {
            Message = message;
        }
    }
}