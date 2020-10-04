using System.Threading.Tasks;
using Discord;
using DiscordManager.Command;
using DiscordManager.Interfaces;

namespace DiscordManager.TestApp
{
  public class Test : CommandModule
  {
    [CommandName("Test")]
    public async void TestMethod()
    {
      var obj = Manager.GetObject<TestObj>();
      await ReplyAsync(obj.Yea);
    }

    [HelpMethod("Test")]
    public async void Help()
    {
      var eb = new EmbedBuilder();
      eb.WithTitle("This is Test Help");
      await ReplyAsync(embed: eb.Build());
    }
  }
}