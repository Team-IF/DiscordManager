using Discord.WebSocket;

namespace DiscordManager.Command
{
    public class Context
    {
        protected BaseSocketClient Client { get; private set; }
        public SocketMessage Message { get; private set; }
        /// <summary>
        /// Get Message Author
        /// </summary>
        public SocketUser Author => Message.Author;
        /// <summary>
        /// Get Channel from Message
        /// </summary>
        public ISocketMessageChannel Channel => Message.Channel;
        /// <summary>
        /// If message from guild Not Null
        /// Opposition is guild not null
        /// </summary>
        public SocketGuild? Guild => (Channel as SocketGuildChannel)?.Guild;

        internal void SetClient(BaseSocketClient message)
        {
            Client = message;
        }
        internal void SetMessage(SocketMessage message)
        {
            Message = message;
        }
    }
}