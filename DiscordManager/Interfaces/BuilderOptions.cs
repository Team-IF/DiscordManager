using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordManager.Interfaces
{
    public abstract class BuilderOptions
    {
        internal string Token { get; set; }
        internal TokenType TokenType { get; set; }
        internal UserStatus UserStatus { get; set; }
        internal Game Activity { get; set; }
        internal DiscordSocketConfig Config { get; set; }
        internal BaseDiscordClient Client { get; set; }
    }
}