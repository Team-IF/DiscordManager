using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace DiscordManager.Event
{
  internal class Event<T> where T : class
  {
    private readonly object _lockOnly = new object();
    private ImmutableArray<T> _array;

    public Event()
    {
      _array = ImmutableArray<T>.Empty;
    }

    public bool HasEvents => !_array.IsEmpty;
    public IReadOnlyList<T> Events => _array;

    public void Add(T @event)
    {
      Checker.NotNull(@event, nameof(@event));
      lock (_lockOnly)
      {
        _array = _array.Add(@event);
      }
    }

    public void Remove(T @event)
    {
      Checker.NotNull(@event, nameof(@event));
      lock (_lockOnly)
      {
        _array = _array.Remove(@event);
      }
    }
  }

  internal static class EventExtensions
  {
    public static async Task Invoke(this Event<Func<Task>> eventHandler)
    {
      if (!eventHandler.HasEvents)
        return;
      var events = eventHandler.Events;
      for (var i = 0; i < events.Count; i++) await events[i].Invoke().ConfigureAwait(false);
    }

    public static async Task Invoke<T>(this Event<Func<T, Task>> eventHandler, T arg)
    {
      if (!eventHandler.HasEvents)
        return;
      var events = eventHandler.Events;
      for (var i = 0; i < events.Count; i++) await events[i].Invoke(arg).ConfigureAwait(false);
    }
  }
}