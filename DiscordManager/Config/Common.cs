using System.Collections.Generic;
using DiscordManager.Interfaces;

namespace DiscordManager.Config
{
  [ConfigExtension(ConfigType.TOML)]
  public class Common : IConfig
  {
    public string Token { get; set; } = "";
    public string Prefix { get; set; } = "!";
    public List<ulong> Owners { get; set; } = new List<ulong>();
  }
}