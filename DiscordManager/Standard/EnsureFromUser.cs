﻿using System.Threading.Tasks;
using Discord;
using DiscordManager.Command;

namespace DiscordManager.Standard
{
  public class EnsureFromUser : IStandard<IMessage>
  {
    public Task<bool> CheckAsync(Context context, IMessage param)
    {
      var ok = context.Author.Id == param.Author.Id;
      return Task.FromResult(ok);
    }
  }
}