namespace DiscordManager.Command
{
  public class CommandInfo
  {
    public readonly string ClassName;
    public readonly string[] CommandNames;
    public readonly string MethodName;
    public readonly Permission Permission;
    public readonly Usage Usage;

    public CommandInfo(string className, string methodName, string[] commandNames, Permission permission, Usage usage)
    {
      ClassName = className;
      MethodName = methodName;
      CommandNames = commandNames;
      Permission = permission;
      Usage = usage;
    }
  }
}