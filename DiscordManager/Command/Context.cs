using Discord.WebSocket;

namespace DiscordManager.Command
{
    public class Context
    {
        protected SocketMessage Message { get; private set; }
        protected SocketUser Author => Message.Author;
        protected ISocketMessageChannel Channel => Message.Channel;
        protected SocketGuild Guild => (Channel as SocketGuildChannel)?.Guild;

        internal void SetMessage(SocketMessage message)
        {
            Message = message;
        }
    }
}