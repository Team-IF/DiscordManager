using Discord;
using DiscordManager.Command;

namespace DiscordManager.TestApp
{
    public class Test : CommandModule
    {
        [CommandName("Test")]
        public void TestMethod()
        {
            var restUserMessage = ReplyAsync("Test").Result;
            var res = NextEmojiAsync(restUserMessage, new IEmote[]{new Emoji("🇾")}).Result;
            ReplyAsync(res.Name);
        }
    }
}