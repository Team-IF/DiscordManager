using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordManager.Command
{
    /// <summary>
    /// Use for Command System
    /// Command Module must be extends this class
    /// </summary>
    public abstract class CommandModule : Context
    {
        protected async Task<RestUserMessage> ReplyAsync(string text)
        {
            return await Channel.SendMessageAsync(text).ConfigureAwait(false);
        }
    }
}