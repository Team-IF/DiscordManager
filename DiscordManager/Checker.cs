using System;

namespace DiscordManager
{
    internal static class Checker
    {
        /// <summary>
        /// Check obj is null
        /// </summary>
        /// <param name="obj">Object to check</param>
        /// <param name="name">Object Name</param>
        /// <param name="msg">Message</param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException">If obj is null</exception>
        public static void NotNull<T>(T obj, string name, string msg = null) where T : class
        {
            if (obj == null) throw CreateNullException(name, msg);
        }

        private static ManagerArgumentException CreateNullException(string name, string msg)
        {
            return msg == null ? new ManagerArgumentException(name) : new ManagerArgumentException(name, msg);
        }
    }
}