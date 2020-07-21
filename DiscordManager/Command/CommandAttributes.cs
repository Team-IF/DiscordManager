using System;
using System.Linq;
using Discord;

namespace DiscordManager.Command
{
    /// <summary>
    /// Use For Command Method
    /// Set Command Name or Command Names
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CommandName : Attribute
    {
        internal readonly string[] Names;

        public CommandName(params string[] commandName)
        {
            Names = commandName;
        }
    }

    /// <summary>
    /// Use For Command Usage
    /// Can set where to use the command
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CommandUsage : Attribute
    {
        internal readonly Usage Usage;
        public CommandUsage(Usage usage)
        {
            Usage = usage;
        }
    }
    
    /// <summary>
    /// Use For Command Method
    /// Set Require Permission for Bot execute this Command
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class RequireBotPermission : Attribute
    {
        internal readonly GuildPermission[] Permissions;

        public RequireBotPermission(params GuildPermission[] permissions)
        {
            Permissions = permissions;
        }
    }

    /// <summary>
    /// Use For Command Method
    /// Set Require Permission for User execute this Command
    /// </summary>
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