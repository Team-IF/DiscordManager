using Discord;
using DiscordManager.Command;

namespace DiscordManager.TestApp
{
    public class Test : CommandModule
    {
        [CommandName("Test"), RequirePermission(Permission.User)]
        public async void TestMethod(int args)
        {
            await ReplyAsync(args.ToString());
        }
    }
}