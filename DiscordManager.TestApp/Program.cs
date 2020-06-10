using System;

namespace DiscordManager.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DiscordBuilder.UseSocketBuilder()
                .WithToken("test")
                .Build();
        }
    }
}