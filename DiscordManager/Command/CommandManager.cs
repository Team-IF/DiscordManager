using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordManager.Config;
using DiscordManager.Logging;

namespace DiscordManager.Command
{
  public static class CommandManager
  {
    private static readonly Logger CommandLogger =
      DiscordManager.Manager.LogManager.CreateLogger("Command Manager (CM)");

    private static IReadOnlyDictionary<Context, IReadOnlyCollection<CommandWrapper>> _commands;

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
      if (e.Author.Id == DiscordManager.Manager.GetConfig<Common>().Owner)
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

    internal static void LoadCommands(BaseSocketClient client)
    {
      var assembly = AppDomain.CurrentDomain.GetAssemblies();
      var types = assembly.SelectMany(s => s.GetTypes())
        .Where(p => !p.IsAbstract && p.IsClass && typeof(CommandModule).IsAssignableFrom(p))
        .ToList();
      var commands = new Dictionary<Context, IReadOnlyCollection<CommandWrapper>>();
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
          if (!(Attribute.GetCustomAttribute(method, typeof(CommandName), true) is CommandName commandName))
            throw new ManagerException($"{method.Name} has None CommandName Attribute.");

          if (commandName.Names.Any(name => nameList.Any(names => names.Contains(name))))
            throw new ManagerException($"{method.Name} has Overlap CommandName");
          ;
          var botPermission =
            Attribute.GetCustomAttribute(method, typeof(RequireBotPermission), true) as RequireBotPermission;
          var requirePermission =
            Attribute.GetCustomAttribute(method, typeof(RequirePermission), true) as RequirePermission;
          var usage =
            ((CommandUsage) Attribute.GetCustomAttribute(method, typeof(CommandUsage), true))?.Usage ?? Usage.ALL;

          list.Add(new CommandWrapper(commandName, usage, requirePermission, botPermission, method));
        }

        var construct = (Context) Activator.CreateInstance(type);
        construct.SetClient(client);
        commands.Add(construct, list);
      }

      _commands = commands;
    }

    public static async void ExecuteCommand(SocketMessage message, string commandName, params object[] param)
    {
      var valuePair = GetCommand(commandName);
      if (valuePair == null)
        return;
      var channel = message.Channel;
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

      var baseClass = valuePair.Value.Key;
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

        baseClass.SetMessage(message);
        await CommandLogger.InfoAsync($"Command Method Execute : {service.Name}").ConfigureAwait(false);
        try
        {
          var parameters = service.GetParameters();
          service.Invoke(baseClass, parameters.Length == 0 ? null : param);
        }
        catch (Exception e)
        {
          await CommandLogger.ErrorAsync("Error At Executing Command", e);
          throw;
        }
      });
      try
      {
        await task;
      }
      catch
      {
        await CommandLogger.ErrorAsync("Error At Executing Command", task.Exception);
      }
    }
  }
}