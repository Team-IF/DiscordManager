namespace DiscordManager.Event
{
    internal class FuncManager<T> where T : class
    {
        private readonly object _lockOnly = new object();

        public FuncManager()
        {
        }
    }

    internal static class EventManagerExtensions
    {
        
    }
}