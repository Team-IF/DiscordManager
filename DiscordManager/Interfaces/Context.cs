using Discord.WebSocket;
using DiscordManager.Logging;

namespace DiscordManager.Interfaces
{
  public class Context
  {
    protected DiscordManager Manager = DiscordManager.Manager;
    protected BaseSocketClient Client => DiscordManager.Manager.GetClient();
    protected SocketMessage Message { get; private set; }
    internal SocketMessage _message
    {
      set => Message = value;
    }

    /// <summary>
    ///   Get Message Author from Message
    /// </summary>
    public SocketUser Author => Message.Author;

    /// <summary>
    ///   Get Message Channel from Message
    /// </summary>
    public ISocketMessageChannel Channel => Message.Channel;

    /// <summary>
    ///   If message from guild Not Null
    ///   Opposition is guild not null
    /// </summary>
    public SocketGuild? Guild => (Channel as SocketGuildChannel)?.Guild;
  }
}