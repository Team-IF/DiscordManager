using Discord;
using Discord.WebSocket;

namespace DiscordManager.Interfaces
{
    internal interface IDiscordBuilder
    {
        /// <summary>
        /// Can be Add DiscordSocketClient manually
        /// </summary>
        /// <param name="client">Your DiscordSocketClient</param>
        /// <returns></returns>
        DiscordSocketBuilder SetDiscordClient(DiscordSocketClient client);
    }
}