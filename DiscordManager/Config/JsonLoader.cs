using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace DiscordManager.Config
{
  internal static class JsonLoader
  {
    public static object GetJson(string fileName, Type type)
    {
      var text = File.ReadAllText($"{LoaderConfig.Path}/{fileName}");
      return JsonConvert.DeserializeObject(text, type);
    }

    public static void SaveJson(string fileName, object obj, Type type)
    {
      try
      {
        File.WriteAllText($"{LoaderConfig.Path}/{fileName}",
          JsonConvert.SerializeObject(obj, type, Formatting.Indented, new JsonSerializerSettings()), Encoding.UTF8);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }
  }
}