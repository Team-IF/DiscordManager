using System;
using Discord;
using Discord.WebSocket;
using DiscordManager.Interfaces;

namespace DiscordManager
{
    /// <summary>
    /// It's Simple Discord Builder
    /// </summary>
    public class DiscordBuilder
    {
        private DiscordBuilder()
        {
        }

        /// <summary>
        /// If you want to use DiscordShardedClient
        /// </summary>
        /// <returns>Discord Shard Builder</returns>
        public static DiscordShardBuilder UseShardBuilder()
        {
            return new DiscordShardBuilder();
        }

        /// <summary>
        /// If you want to use DiscordSocketClient 
        /// </summary>
        /// <returns>Discord Socket Builder</returns>
        public static DiscordSocketBuilder UseSocketBuilder()
        {
            return new DiscordSocketBuilder();
        }
    }

    public class DiscordShardBuilder : CommonBuilder<DiscordShardBuilder>, IDiscordShardBuilder
    {
        internal DiscordShardBuilder()
        {
            Instance = this;
        }

        public DiscordShardBuilder SetDiscordClient(DiscordShardedClient client)
        {
            Client = client;
            return this;
        }
    }

    public class DiscordSocketBuilder : CommonBuilder<DiscordSocketBuilder>, IDiscordBuilder
    {
        internal DiscordSocketBuilder()
        {
            Instance = this;
        }
        
        public DiscordSocketBuilder SetDiscordClient(DiscordSocketClient client)
        {
            Client = client;
            return this;
        }
    }
}