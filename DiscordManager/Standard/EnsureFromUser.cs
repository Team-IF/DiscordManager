using System.Threading.Tasks;
using Discord;
using DiscordManager.Interfaces;

namespace DiscordManager.Standard
{
  internal class EnsureFromUser : IStandard<IMessage>
  {
    public Task<bool> CheckAsync(Context context, IMessage param)
    {
      var ok = context.Author.Id == param.Author.Id;
      return Task.FromResult(ok);
    }
  }
}