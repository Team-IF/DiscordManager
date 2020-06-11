using System;
using System.Collections.Generic;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace DiscordManager
{
    /// <summary>
    /// It's DiscordManager Core
    /// </summary>
    public class DiscordManager
    {
        private BaseSocketClient Client;

        internal DiscordManager(Dictionary<string, object> options)
        {
            var token = (string) (options["Token"] ??
                              throw new DiscordManagerException(
                                  "Token not specified. The token must not be null, use the WithToken method"));
            var tokenType = (TokenType) (options["TokenType"] ?? TokenType.Bot);
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

        internal void Run()
        {
        }
    }
}