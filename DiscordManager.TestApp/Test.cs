using Discord;
using DiscordManager.Command;

namespace DiscordManager.TestApp
{
    public class Test : CommandModule
    {
        [CommandName("Test"), RequirePermission(Permission.Admin)]
        public async void TestMethod(string[] args)
        {
            await ReplyAsync(Manager.GetObject<TestObj>().Yea);
        }
    }
}