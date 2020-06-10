using Discord.WebSocket;

namespace DiscordManager.Interfaces
{
    internal interface IDiscordShardBuilder
    {
        /// <summary>
        /// Can be Add DiscordShardedClient manually
        /// </summary>
        /// <param name="client">Your DiscordShardedClient</param>
        /// <returns></returns>
        DiscordShardBuilder SetDiscordClient(DiscordShardedClient client);
    }
}