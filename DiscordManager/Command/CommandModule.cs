using System.Threading.Tasks;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordManager.Command
{
    public abstract class CommandModule : Context
    {
        protected RestUserMessage Reply(string text)
        {
            return Channel.SendMessageAsync(text).ConfigureAwait(false)
                .GetAwaiter().GetResult();
        }
    }
}