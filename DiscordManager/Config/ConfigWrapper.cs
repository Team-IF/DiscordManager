namespace DiscordManager.Config
{
  internal class ConfigWrapper
  {
    public readonly ConfigType Type;
    public readonly object Target;

    public ConfigWrapper(ConfigType type, object target)
    {
      Type = type;
      Target = target;
    }
  }
}