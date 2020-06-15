using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DiscordManager.Event;

namespace DiscordManager
{
    /// <summary>
    /// It's DiscordManager Core
    /// </summary>
    public class DiscordManager : Events
    {
        private BaseSocketClient Client;

        internal DiscordManager(IReadOnlyDictionary<string, object> options)
        {
            var token = (string) (options["Token"] ??
                              throw new ManagerException(
                                  "Token not specified. The token must not be null, use the WithToken method"));
            var tokenType = (TokenType) (options["TokenType"] ?? TokenType.Bot);
            try
            {
                TokenUtils.ValidateToken(tokenType, token);
            }
            catch (Exception e)
            {
                throw new ManagerException(
                    "");
            }
            var userStatus = (UserStatus) (options["Status"] ?? UserStatus.Online);
            var game = (Game) options["Game"];
            var totalShards = (int?) options["TotalShards"];
            Client = (BaseSocketClient) options["Client"];
            if (Client == null)
            {
                var socketConfig = (DiscordSocketConfig) options["SocketConfig"] ?? new DiscordSocketConfig {MessageCacheSize = 100, TotalShards = totalShards};
                if (totalShards.HasValue)
                    Client = new DiscordShardedClient(socketConfig);
                else
                    Client = new DiscordSocketClient(socketConfig);
            }
        }

        internal void RegisterEvents()
        {
            
        }

        public void Run()
        {
        }
    }
}