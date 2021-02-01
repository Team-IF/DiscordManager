using Discord;
using Discord.WebSocket;

namespace DiscordManager
{
  public static class Extension
  {
    /// <summary>
    ///   return value is "username#discriminator"
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static string GetFullName(this IUser user)
    {
      return $"{user.Username}#{user.Discriminator}";
    }

    /// <summary>
    ///   return value is current user(logged in as token)
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static ISelfUser GetCurrentUser(this BaseSocketClient client)
    {
      return (ISelfUser) client.CurrentUser ?? client.Rest.CurrentUser;
    }
  }
}