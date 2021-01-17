using DiscordManager.Command;
using DiscordManager.Interfaces;

namespace DiscordManager.TestApp
{
  public class Test : CommandModule
  {
    [CommandName("Test")]
    public async void TestMethod()
    {
      var config = Manager.GetConfig<Verify>();
      config.Test.Add("test", "test");
      
      Manager.SaveConfig<Verify>();
    }
  }
}