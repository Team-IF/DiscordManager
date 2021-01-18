using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordManager.Config;
using DiscordManager.Interfaces;
using DiscordManager.Logging;

namespace DiscordManager.Command
{
  public static class CommandManager
  {
    private static readonly Logger CommandLogger =
      DiscordManager.Manager.LogManager.CreateLogger("Command Manager (CM)");

    private static IReadOnlyDictionary<Context, IReadOnlyCollection<CommandWrapper>> _commands;
    private static IReadOnlyDictionary<string, MethodInfo> _helpCommands;
    private static string[] _helpArg;
    private static readonly Regex _contentRegex = new Regex(@"(""[^""]+""|[^\s""]+)");


    private static KeyValuePair<Context, CommandWrapper>? GetCommand(string commandName)
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
      if (DiscordManager.Manager.GetConfig<Common>().Owners.Contains(e.Author.Id))
        return Permission.Owner;
      var customPerm = DiscordManager.Manager.Permission;
      if (customPerm != null)
        return customPerm.Invoke(e);
      if (e.Author is SocketGuildUser guildUser)
        return guildUser.Roles.Any(role => role.Permissions.Administrator)
          ? Permission.Admin
          : Permission.User;

      return Permission.User;
    }

    internal static void LoadCommands(string[] commandConfigHelpArg)
    {
      _helpArg = commandConfigHelpArg;
      var assembly = AppDomain.CurrentDomain.GetAssemblies();
      var types = assembly.SelectMany(s => s.GetTypes())
        .Where(p => !p.IsAbstract && p.IsClass && typeof(CommandModule).IsAssignableFrom(p))
        .ToList();
      var commands = new Dictionary<Context, IReadOnlyCollection<CommandWrapper>>();
      var helpCommands = new Dictionary<string, MethodInfo>();
      for (var i = 0; i < types.Count; i++)
      {
        var type = types[i];
        var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
        var nameList = new List<string[]>();
        var list = new List<CommandWrapper>();
        for (var j = 0; j < methods.Length; j++)
        {
          var method = methods[j];
          if (!method.IsPublic)
            continue;
          if (Attribute.GetCustomAttribute(method, typeof(NotMapping), true) is NotMapping)
            continue;

          if (Attribute.GetCustomAttribute(method, typeof(HelpMethod), true) is HelpMethod helpMethod)
          {
            try
            {
              if (!nameList.Any(names => names.Contains(helpMethod.TargetMethod)))
                helpCommands.Add(helpMethod.TargetMethod, method);
            }
            catch (Exception e)
            {
              CommandLogger.CriticalAsync("CM(Command Manager) Error", e);
              throw;
            }

            continue;
          }

          if (!(Attribute.GetCustomAttribute(method, typeof(CommandName), true) is CommandName commandName))
            throw new ManagerException($"{method.Name} doesn't have CommandName Attribute");

          if (commandName.Names.Any(name => nameList.Any(names => names.Contains(name))))
            throw new ManagerException($"{method.Name} has Overlap CommandName");

          var commandGroup = Attribute.GetCustomAttribute(method, typeof(CommandGroup), true) as CommandGroup;
          var botPermission =
            Attribute.GetCustomAttribute(method, typeof(RequireBotPermission), true) as RequireBotPermission;
          var requirePermission =
            Attribute.GetCustomAttribute(method, typeof(RequirePermission), true) as RequirePermission;
          var usage =
            ((CommandUsage) Attribute.GetCustomAttribute(method, typeof(CommandUsage), true))?.Usage ?? Usage.ALL;

          list.Add(new CommandWrapper(commandName, usage, requirePermission, botPermission, method, commandGroup));
        }

        var construct = (Context) Activator.CreateInstance(type);
        commands.Add(construct, list);
      }

      _helpCommands = helpCommands;
      _commands = commands;
    }

    /// <summary>
    ///  return all Commands
    ///  if CommandGroup is generic not set CommandGroup Attribute Commands
    /// </summary>
    /// <returns>Dict[CommandGroup, Commands]</returns>
    public static Dictionary<string, List<CommandInfo>> GetAllCommands()
    {
      var dict = new Dictionary<string, List<CommandInfo>>();
      for (var i = 0; i < _commands.Count; i++)
      {
        var (key, value) = _commands.ElementAt(i);
        for (var j = 0; j < value.Count; j++)
        {
          var wrapper = value.ElementAt(j);
          var groupName = wrapper.CommandGroup != null ? wrapper.CommandGroup.Group : "Generic";
          if (!dict.TryGetValue(groupName, out var infos))
            infos = new List<CommandInfo>();
          infos.Add(new CommandInfo(key.GetType().Name, wrapper.MethodInfo.Name,
            wrapper.CommandName, wrapper.Permission, wrapper.Usage));
          dict[groupName] = infos;
        }
      }

      return dict;
    }

    public static async void ExecuteCommand(SocketMessage message, string commandName)
    {
      var task = new Task(async () =>
      {
        var valuePair = GetCommand(commandName);
        if (valuePair == null)
          return;
        var channel = message.Channel;
        var baseClass = valuePair.Value.Key;
        var command = valuePair.Value.Value;
        switch (command.Usage)
        {
          case Usage.Guild:
            if (!(channel is SocketGuildChannel))
              return;
            break;
          case Usage.DM:
            if (!(channel is SocketDMChannel))
              return;
            break;
          case Usage.ALL:
            break;
        }

        if (!command.PermCheck(message)) return;
        var matchesCollection = _contentRegex.Matches(message.Content);
        var matches = matchesCollection.Select(mts => mts.Value.Replace("\"", "").Trim()).Skip(1).ToArray();
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

        if (matches.Length != 0 && _helpArg.Contains(matches[0]))
        {
          if (_helpCommands.ContainsKey(command.CommandName[0]))
            service = _helpCommands[command.CommandName[0]];
        }
        baseClass._message = message;
        var parameters = service.GetParameters();
        object?[]? param = null;
        if (parameters.Length != 0)
        {
          param = new object[parameters.Length];
          if (matches.Length != 0)
            for (var i = 0; i < parameters.Length; i++)
            {
              var parameter = parameters[i];
              var parameterType = parameter.ParameterType;
              var count = i + 1;
              if (parameterType.IsArray && count == parameters.Length)
              {
                var elementType = parameterType.GetElementType();
                var paramArray = matches.Skip(i).Where(item =>
                {
                  try
                  {
                    Convert.ChangeType(item, elementType);
                    return true;
                  }
                  catch
                  {
                    // ignored
                  }

                  return false;
                }).Select(item => Convert.ChangeType(item, elementType)).ToArray();

                var destinationArray = Array.CreateInstance(elementType, paramArray.Length);
                Array.Copy(paramArray, destinationArray, paramArray.Length);
                param[i] = destinationArray;
              }
              else
              {
                object? converted = null;
                if (matches.Length > i)
                {
                  var content = matches[i];
                  try
                  {
                    if (parameterType == typeof(string[]))
                      converted = matches;
                    else if (parameterType.IsEnum)
                      converted = Enum.Parse(parameterType, content);
                    else
                      converted = parameterType == typeof(string)
                        ? content
                        : Convert.ChangeType(content, parameterType);
                  }
                  catch (Exception)
                  {
                    if (parameter.HasDefaultValue)
                      converted = parameter.DefaultValue;
                  }
                }

                param[i] = converted;
              }
            }
        }

        service.Invoke(baseClass, param);
        await CommandLogger.InfoAsync($"Command Method Execute : {service.Name}").ConfigureAwait(false);
      });
      try
      {
        task.Start();
        task.Wait();
      }
      catch
      {
        await CommandLogger.DebugAsync("Error At Executing Command", task.Exception).ConfigureAwait(false);
      }
    }
  }
}