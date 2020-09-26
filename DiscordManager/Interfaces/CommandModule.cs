using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordManager.Standard;

namespace DiscordManager.Interfaces
{
  /// <summary>
  ///   Use for Command System
  ///   Command Module must be extends this class
  /// </summary>
  public abstract class CommandModule : Context
  {
    private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(3);

    protected async Task<RestUserMessage> ReplyAsync(string text = null, Embed embed = null, bool isTTS = false)
    {
      return await Channel.SendMessageAsync(text, isTTS, embed).ConfigureAwait(false);
    }

    protected async Task<IEmote?> NextEmojiAsync(RestUserMessage message, IEmote[] emotes, bool catchAny = false,
      TimeSpan? timeOut = null, CancellationToken token = default)
    {
      timeOut ??= _defaultTimeout;

      var eventTrigger = new TaskCompletionSource<IEmote>();
      var cancelTrigger = new TaskCompletionSource<bool>();

      token.Register(() => cancelTrigger.SetResult(true));

      async Task Handler(Cacheable<IUserMessage, ulong> cacheable, ISocketMessageChannel socketMessageChannel,
        SocketReaction arg3)
      {
        if (arg3.MessageId == message.Id && (arg3.UserId == Message.Author.Id || catchAny) &&
            emotes.Contains(arg3.Emote)) eventTrigger.SetResult(arg3.Emote);
      }

      for (var i = 0; i < emotes.Length; i++) _ = message.AddReactionAsync(emotes[i]);

      Client.ReactionAdded += Handler;

      var trigger = eventTrigger.Task;
      var cancel = cancelTrigger.Task;
      var delay = Task.Delay(timeOut.Value);
      var task = await Task.WhenAny(trigger, delay, cancel).ConfigureAwait(false);

      Client.ReactionAdded -= Handler;

      if (task == trigger)
        return await trigger.ConfigureAwait(false);
      return null;
    }

    protected async Task<SocketMessage?> NextMessageAsync(TimeSpan? timeOut = null, bool catchAny = false,
      CancellationToken token = default)
    {
      var standard = new Standard<SocketMessage>();
      if (!catchAny)
        standard.AddCriterion(new EnsureFromUser());
      return await NextMessageAsync(standard, timeOut, token);
    }

    private async Task<SocketMessage?> NextMessageAsync(IStandard<SocketMessage> standard, TimeSpan? timeOut = null,
      CancellationToken token = default)
    {
      timeOut ??= _defaultTimeout;

      var eventTrigger = new TaskCompletionSource<SocketMessage>();
      var cancelTrigger = new TaskCompletionSource<bool>();

      token.Register(() => cancelTrigger.SetResult(true));

      async Task Handler(SocketMessage message)
      {
        var result = await standard.CheckAsync(this, message).ConfigureAwait(false);
        if (result)
          eventTrigger.SetResult(message);
      }

      Client.MessageReceived += Handler;

      var trigger = eventTrigger.Task;
      var cancel = cancelTrigger.Task;
      var delay = Task.Delay(timeOut.Value);
      var task = await Task.WhenAny(trigger, delay, cancel).ConfigureAwait(false);

      Client.MessageReceived -= Handler;

      if (task == trigger)
        return await trigger.ConfigureAwait(false);
      return null;
    }
  }
}