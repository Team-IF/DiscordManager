using System.Collections.Generic;
using System.Net.Sockets;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordManager.Interfaces
{
    public class CommonBuilder<T> where T : CommonBuilder<T>
    {
        internal Dictionary<string, object> Options { get; } = new Dictionary<string, object>();
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
        /// <returns></returns>
        public T WithToken(string token, TokenType tokenType)
        {
            Options["Token"] = token;
            Options["TokenType"] = tokenType;
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
        /// <returns></returns>
        public T WithStatus(UserStatus status)
        {
            Options["Status"] = status;
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
        /// <returns></returns>
        public T WithActivity(Game game)
        {
            Options["Game"] = game;
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
        /// <returns></returns>
        public T UseSocketConfig(DiscordSocketConfig config)
        {
            Options["SocketConfig"] = config;
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
        /// <returns></returns>
        public T SetClient(BaseSocketClient client)
        {
            Options["Client"] = client;
            return (T) this;
        }

        public DiscordManager Build()
        {
            return new DiscordManager(Options);
        }
    }
}