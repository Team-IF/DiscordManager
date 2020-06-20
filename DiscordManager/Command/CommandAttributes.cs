using System;
using System.Linq;
using Discord;

namespace DiscordManager.Command
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CommandName : Attribute
    {
        internal readonly string[] Names;

        public CommandName(params string[] commandName)
        {
            Names = commandName;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class RequireBotPermission : Attribute
    {
        internal readonly GuildPermission[] Permissions;

        public RequireBotPermission(params GuildPermission[] permissions)
        {
            Permissions = permissions;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class RequirePermission : Attribute
    {
        public RequirePermission(Permission permission = Permission.User)
        {
            Permission = permission;
        }

        internal readonly Permission Permission;
    }

}