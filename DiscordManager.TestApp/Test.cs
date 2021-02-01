using DiscordManager.Command;
using DiscordManager.Interfaces;

namespace DiscordManager.TestApp
{
  public class Test : CommandModule
  {
    [CommandName("Test")]
    public async void TestMethod()
    {
      await ReplyAsync("Test");
    }
  }
}