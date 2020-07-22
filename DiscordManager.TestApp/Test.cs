using Discord;
using DiscordManager.Command;

namespace DiscordManager.TestApp
{
    public class Test : CommandModule
    {
        [CommandName("Test"), RequirePermission(Permission.Admin)]
        public async void TestMethod(string[] args)
        {
            var replyAsync = await ReplyAsync("Test");
            var targetEmote = new Emoji("🇾");
            var emoji = await NextEmojiAsync(replyAsync, new IEmote[] {targetEmote, new Emoji("🇳")});
            if (Equals(targetEmote, emoji))
                await ReplyAsync(emoji.Name);
        }
    }
}