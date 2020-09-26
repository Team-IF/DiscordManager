using System.Threading.Tasks;
using Discord;
using DiscordManager.Command;
using DiscordManager.Interfaces;

namespace DiscordManager.TestApp
{
  public class Test : CommandModule
  {
    [CommandName("Test"), RequirePermission(Permission.User)]
    public async void TestMethod(int? args)
    {
      await ReplyAsync(args.ToString());
    }

    public async void Test_help()
    {
      var eb = new EmbedBuilder();
      eb.WithTitle("This is Test Help");
      await ReplyAsync(embed: eb.Build());
    }
  }
}