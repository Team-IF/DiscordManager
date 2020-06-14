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

        public static SocketBuilder SocketBuilder => new SocketBuilder();
        public static ShardBuilder ShardBuilder => new ShardBuilder();
    }

    public class SocketBuilder : CommonBuilder<SocketBuilder>
    {
        internal SocketBuilder()
        {
        }
    }

    public class ShardBuilder : CommonBuilder<SocketBuilder>
    {
        internal ShardBuilder()
        {
            WithShard(2);
        }

        /// <summary>
        /// Can set the TotalShard for the bot. If use ShardBuilder, totalShards will use the default value of 2.
        /// </summary>
        /// <param name="totalShards">Total Shard Count</param>
        /// <returns></returns>
        public ShardBuilder WithShard(int totalShards)
        {
            Options["TotalShards"] = totalShards;
            return this;
        }
    }
}