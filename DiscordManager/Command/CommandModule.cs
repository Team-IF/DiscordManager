using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordManager.Standard;

namespace DiscordManager.Command
{
    /// <summary>
    /// Use for Command System
    /// Command Module must be extends this class
    /// </summary>
    public abstract class CommandModule : Context
    {
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(3);
        protected async Task<RestUserMessage> ReplyAsync(string text, Embed embed = null, bool isTTS = false)
        {
            return await Channel.SendMessageAsync(text, isTTS, embed).ConfigureAwait(false);
        }

        protected async Task<SocketMessage?> NextMessageAsync(TimeSpan? timeOut = null, bool catchAny = false, CancellationToken token = default)
        {
            var standard = new Standard<SocketMessage>();
            if (!catchAny)
                standard.AddCriterion(new EnsureFromUser());
            return await NextMessageAsync(standard, timeOut, token);
        }

        protected async Task<SocketMessage?> NextMessageAsync(IStandard<SocketMessage> standard, TimeSpan? timeOut = null, CancellationToken token = default)
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