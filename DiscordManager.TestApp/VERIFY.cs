using System.Collections.Generic;
using DiscordManager.Config;
using DiscordManager.Interfaces;

namespace DiscordManager.TestApp
{
  [ConfigExtension(ConfigType.JSON)]
  public class Verify : IConfig
  {
    public Dictionary<string, string> Test { get; set; } = new Dictionary<string, string>();
  }
}