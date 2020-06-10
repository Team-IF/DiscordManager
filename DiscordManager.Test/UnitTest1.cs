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
            DiscordBuilder.UseSocketBuilder()
                .WithToken("test2")
                .Build();
        }
    }
}