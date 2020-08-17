using System;

namespace DiscordManager.Config
{
  /// <summary>
  ///   Set Config Extension
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public class ConfigExtension : Attribute
  {
    internal readonly ConfigType Type;

    public ConfigExtension(ConfigType type)
    {
      Type = type;
    }
  }
}