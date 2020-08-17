using System;
using System.Linq;
using System.Threading.Tasks;
using Nett;

namespace DiscordManager.Config
{
  internal static class TomlLoader
  {
    public static object GetToml(string fileName, Type type)
    {
      try
      {
        var toml = typeof(Toml);
        var method = toml.GetMethods().Single(m => m.Name == "ReadFile" &&
                                                   m.GetGenericArguments().Length == 1 &&
                                                   m.GetParameters().Length == 1 &&
                                                   m.GetParameters()[0].ParameterType == typeof(string));
        method = method.MakeGenericMethod(type);
        return method.Invoke(null, new object[] {$"{LoaderConfig.Path}/{fileName}"});
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    public static Task SaveToml(string fileName, object obj)
    {
      Toml.WriteFile(obj, $"{LoaderConfig.Path}/{fileName}");
      return Task.CompletedTask;
    }
  }
}