namespace DiscordManager.Config
{
  [ConfigExtension(ConfigType.TOML)]
  public class Common : IConfig
  {
    public string Token { get; set; } = "";
    public string Prefix { get; set; } = "!";
    public ulong Owner { get; set; } = 0;
  }
}