using System.Linq;
using System.Reflection;
using Discord;

namespace DiscordManager.Command
{
    internal class CommandWrapper
    {
        public CommandWrapper(CommandName commandName, RequirePermission permission, RequireBotPermission botPermission, MethodInfo info)
        {
            _commandName = commandName.Names;
            Permission = permission?.Permission ?? Permission.User;
            BotPermission = botPermission?.Permissions;
            MethodInfo = info;
        }

        private readonly string[] _commandName;
        public Permission Permission { get; }
        public readonly GuildPermission[]? BotPermission;
        public readonly MethodInfo MethodInfo;
        
        public bool Contains(string name)
        {
            return _commandName.Contains(name);
        }
        
        /// <summary>
        ///     Check Permission
        /// </summary>
        /// <returns>return Missing Permissions But if Empty doesn't have Missing Permissions</returns>
        public GuildPermission[] CheckPermissions(IGuildUser currentUser)
        {
            var currentPermission = currentUser.GuildPermissions;
            if (currentPermission.Administrator)
                return null;
            var missingPerms = BotPermission.Where(permission => !currentPermission.Has(permission)).ToArray();
            return missingPerms.Length == 0 ? null : missingPerms;
        }
    }
}