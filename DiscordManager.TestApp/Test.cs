using DiscordManager.Command;

namespace DiscordManager.TestApp
{
    public class Test : CommandModule
    {
        [CommandName("Test")]
        public void TestMethod()
        {
            _ = Reply("Test");
        }
    }
}