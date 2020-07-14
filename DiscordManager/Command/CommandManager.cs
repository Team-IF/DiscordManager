using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordManager.Logging;

namespace DiscordManager.Command
{
    public static class CommandManager
    {
        internal static Logger _commandLogger;
        private static IReadOnlyDictionary<object, IReadOnlyCollection<CommandWrapper>> _commands;

        private static KeyValuePair<object, CommandWrapper>? GetCommand(string commandName)
        {
            for (var i = 0; i < _commands.Count; i++)
            {
                var (key, value) = _commands.ElementAt(i);
                for (var j = 0; j < value.Count; j++)
                {
                    var commandWrapper = value.ElementAt(j);
                    if (commandWrapper.Contains(commandName))
                        return KeyValuePair.Create(key, commandWrapper);
                }
            }

            return null;
        }
        
        private static bool PermissionFilter(Permission user, Permission command)
        {
            return user <= command;
        }

        private static bool PermCheck<T>(this T source, SocketMessage e) where T : CommandWrapper
        {
            return PermissionFilter(GetPermission(e), source.Permission);
        }
        
        private static Permission GetPermission(SocketMessage e)
        {
            if (e.Author is SocketGuildUser guildUser)
            {
                return guildUser.Roles.Any(role => role.Permissions.Administrator)
                    ? Permission.Admin
                    : Permission.User;
            }

            return Permission.User;
        }
        
        internal static void LoadCommands()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies();
            var types = assembly.SelectMany(s => s.GetTypes())
                .Where(p => !p.IsAbstract && typeof(CommandModule).IsAssignableFrom(p))
                .ToList();
            var commands = new Dictionary<object, IReadOnlyCollection<CommandWrapper>>();
            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i];
                if (!type.IsClass)
                    continue;
                var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
                var nameList = new List<string[]>();
                var list = new List<CommandWrapper>();
                for (var j = 0; j < methods.Length; j++)
                {
                    var method = methods[j];
                    if (!method.IsPublic)
                        continue;
                    if (!(Attribute.GetCustomAttribute(method, typeof(CommandName), true) is CommandName commandName))
                        throw new ManagerException($"{method.Name} has None CommandName Attribute.");
                    
                    if (commandName.Names.Any(name => nameList.Any(names => names.Contains(name))))
                        throw new ManagerException($"{method.Name} has Overlap CommandName");;
                    var botPermission =
                        Attribute.GetCustomAttribute(method, typeof(RequireBotPermission), true) as RequireBotPermission;
                    var requirePermission =
                        Attribute.GetCustomAttribute(method, typeof(RequirePermission), true) as RequirePermission;
                    list.Add(new CommandWrapper(commandName, requirePermission, botPermission, method));
                }
                var construct = Activator.CreateInstance(type);
                commands.Add(construct, list);
            }

            _commands = commands;
        }

        public static async void ExecuteCommand(SocketMessage message, string commandName, params object[] args)
        {
            var valuePair = GetCommand(commandName);
            if (valuePair == null)
                return;
            var command = valuePair.Value.Value;
            var baseClass = (Context) valuePair.Value.Key;
            baseClass.SetMessage(message);
            var channel = message.Channel;
            var task = Task.Run(async () =>
            {
                if (!command.PermCheck(message)) return;
                var service = command.MethodInfo;
                var perm = command.BotPermission;
                if (channel is SocketGuildChannel guildChannel && perm != null)
                {
                    var missingPerms = command.CheckPermissions(guildChannel.Guild.CurrentUser);
                    if (missingPerms != null)
                    {
                        var missingP = string.Join(", ", missingPerms);
                        await channel.SendMessageAsync(
                            $"Missing Bot Permission : {missingP}\nPlease Add Missing Permissions");
                        return;
                    }
                }
                service.Invoke(baseClass, args);
            });
            try
            {
                await task;
            }
            catch
            {
                await _commandLogger.ErrorAsync("Error At Executing Command", task.Exception);
            }
        }
    }
}