using System;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordManager.Interfaces
{
  public class CommandConfig
  {
    public Func<SocketMessage, Task> CommandFunc;
    public string[] HelpArg = {"help"};

    internal static CommandConfig GetDefault()
    {
      return new CommandConfig();
    }
  }
}