using System.Collections.Generic;
using System.Net.Sockets;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordManager.Logging;

namespace DiscordManager.Interfaces
{
    public class CommonBuilder<T> where T : CommonBuilder<T>
    {
        internal CommonBuilder()
        {
            Option = new BuildOption();
        }
        internal readonly BuildOption Option;

        /// <summary>
        /// Use the Command Service in DiscordManager
        /// </summary>
        public T WithCommandModule(string prefix  = "!")
        {
            Option.UseCommandModule = true;
            Option.Prefix = prefix;
            return (T) this;
        }
        /// <summary>
        /// Set LogLevel(NONE,INFO, ERROR, CRITICAL,DEBUG, ALL)
        /// </summary>
        /// <param name="level">Log Level for DiscordManager</param>
        public T WithLogLevel(LogLevel level)
        {
            Option.LogLevel = level;
            return (T) this;
        }
        /// <summary>
        /// It's sets the bot token and TokenType.
        /// TokenType Default Setting is <strong>Bot</strong>
        /// <example> For Example
        /// <code>
        ///    Builder.WithToken("Token");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="status">Status Type</param>
        public T WithToken(TokenType tokenType)
        {
            Option.Type = tokenType;
            return (T)this;
        }

        /// <summary>
        /// Can set the Status for the bot
        /// Default Setting is <strong>Online</strong>
        /// <example> For Example
        /// <code>
        ///    Builder.WithStatus(UserStatus.Online);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="status">Status Type</param>
        public T WithStatus(UserStatus status)
        {
            Option.BotStatus = status;
            return (T)this;
        }

        /// <summary>
        /// Can set the status message for the bot
        /// <example> For Example
        /// <code>
        ///    Builder.WithActivity(new Game("Test!!"));
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="game">Status Message</param>
        public T WithActivity(Game game)
        {
            Option.Game = game;
            return (T)this;
        }

        /// <summary>
        /// Can set the DiscordSocketConfig for the bot
        /// <example> For Example
        /// <code>
        ///    Builder.UseSocketConfig(new DiscordSocketConfig {MessageCacheSize = 100});
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="config">Discord Socket Config</param>
        public T UseSocketConfig(DiscordSocketConfig config)
        {
            Option.SocketConfig = config;
            return (T)this;
        }

        /// <summary>
        /// Can set the DiscordClient for the bot
        /// <example> For Example
        /// <code>
        ///    Builder.SetClient(new DiscordSocketClient());
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="client">Discord Client</param>
        public T SetClient(BaseSocketClient client)
        {
            Option.Client = client;
            return (T) this;
        }

        /// <summary>
        /// Builder to Discord Manager
        /// </summary>
        public DiscordManager Build()
        {
            return new DiscordManager(Option);
        }
    }
}