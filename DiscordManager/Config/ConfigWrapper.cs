namespace DiscordManager.Config
{
  internal class ConfigWrapper
  {
    public readonly object Target;
    public readonly ConfigType Type;

    public ConfigWrapper(ConfigType type, object target)
    {
      Type = type;
      Target = target;
    }
  }
}