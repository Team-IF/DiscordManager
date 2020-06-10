using System.Net.Sockets;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordManager.Interfaces
{
    public class CommonBuilder<T> : BuilderOptions where T : CommonBuilder<T>
    {
        internal T Instance { get; set; }

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
        public T WithToken(string token, TokenType tokenType = TokenType.Bot)
        {
            Token = token;
            TokenType = tokenType;
            return Instance;
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
            UserStatus = status;
            return Instance;
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
            Activity = game;
            return Instance;
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
            Config = config;
            return Instance;
        }

        public DiscordManager Build()
        {
            return new DiscordManager(this);
        }
    }
}