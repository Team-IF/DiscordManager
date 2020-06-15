using NUnit.Framework;

namespace DiscordManager.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var discordManager = DiscordBuilder.SocketBuilder.Build();
        }
    }
}