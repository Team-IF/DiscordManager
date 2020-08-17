using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordManager.Config
{
  internal static class LoaderConfig
  {
    public static string Path;
    
    public static async Task CreateFile(string fileName, Type type, ConfigType configType)
    {
      var fileInfo = new FileInfo($"{Path}/{fileName}");
      await CreateDir(fileInfo.Directory);
      if (!fileInfo.Exists)
      {
        var instance = Activator.CreateInstance(type);
        if (configType == ConfigType.JSON)
          JsonLoader.SaveJson(fileName, instance, type);
        else 
          await TomlLoader.SaveToml(fileName, instance);
      }
    }

    private static Task CreateDir(DirectoryInfo dirInfo)
    {
      if (!dirInfo.Exists)
        dirInfo.Create();
      return Task.CompletedTask;
    }
  }
}