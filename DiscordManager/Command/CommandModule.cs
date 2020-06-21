using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordManager.Command
{
    public abstract class CommandModule : Context
    {
        protected async Task<RestUserMessage> ReplyAsync(string text)
        {
            return await Channel.SendMessageAsync(text).ConfigureAwait(false);
        }
    }
}