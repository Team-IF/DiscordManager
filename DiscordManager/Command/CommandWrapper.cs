using System.Linq;
using System.Reflection;
using Discord;

namespace DiscordManager.Command
{
  internal class CommandWrapper
  {
    public readonly GuildPermission[]? BotPermission;
    public readonly CommandGroup CommandGroup;
    public readonly string[] CommandName;
    public readonly MethodInfo MethodInfo;
    public readonly Permission Permission;
    public readonly Usage Usage;

    public CommandWrapper(CommandName commandName, Usage usage, RequirePermission? permission,
      RequireBotPermission? botPermission, MethodInfo info, CommandGroup commandGroup)
    {
      CommandName = commandName.Names;
      Usage = usage;
      Permission = permission?.Permission ?? Permission.User;
      BotPermission = botPermission?.Permissions;
      MethodInfo = info;
      CommandGroup = commandGroup;
    }

    public bool Contains(string name)
    {
      return CommandName.Contains(name);
    }

    /// <summary>
    ///   Check Permission
    /// </summary>
    /// <returns>return Missing Permissions But if Empty doesn't have Missing Permissions</returns>
    public GuildPermission[]? CheckPermissions(IGuildUser currentUser)
    {
      var currentPermission = currentUser.GuildPermissions;
      if (currentPermission.Administrator)
        return null;
      var missingPerms = BotPermission.Where(permission => !currentPermission.Has(permission)).ToArray();
      return missingPerms.Length == 0 ? null : missingPerms;
    }
  }
}