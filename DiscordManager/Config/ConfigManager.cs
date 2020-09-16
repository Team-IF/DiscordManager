using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordManager.Logging;

namespace DiscordManager.Config
{
  internal class ConfigManager
  {
    private static readonly Logger ConfigLogger = DiscordManager.Manager.LogManager.CreateLogger("Config Loader (CL)");
    private readonly Dictionary<string, ConfigWrapper> _configs;
    private readonly object _lockOnly = new object();

    public ConfigManager()
    {
      _configs = new Dictionary<string, ConfigWrapper>();
    }

    public object this[string configName] => _configs.GetValueOrDefault(configName);

    public T Get<T>() where T : IConfig
    {
      var config = _configs.GetValueOrDefault(typeof(T).Name);
      return (T) config.Target;
    }

    public object Get(object obj)
    {
      return _configs.GetValueOrDefault(nameof(obj));
    }

    public void Save()
    {
      lock (_lockOnly)
      {
        for (var i = 0; i < _configs.Count; i++)
        {
          var (key, value) = _configs.ElementAt(i);

          var target = value.Target;
          var fullName = $"{key}Config.{value.Type.ToString().ToLower()}";
          if (value.Type == ConfigType.JSON)
            JsonLoader.SaveJson(fullName, target, target.GetType());
          else
            TomlLoader.SaveToml(fullName, target).ConfigureAwait(false);
        }
      }
    }

    public void ReLoad()
    {
      lock (_lockOnly)
      {
        _configs.Clear();

        Load().ConfigureAwait(false);
      }
    }

    internal async Task Load()
    {
      var assembly = AppDomain.CurrentDomain.GetAssemblies();
      var types = assembly.SelectMany(s => s.GetTypes())
        .Where(p => p.IsClass && !p.IsAbstract && typeof(IConfig).IsAssignableFrom(p))
        .ToList();
      for (var i = 0; i < types.Count; i++)
      {
        var type = types[i];
        var typeName = type.Name;
        if (_configs.ContainsKey(typeName))
        {
          await ConfigLogger.ErrorAsync($"Duplicate setting name : {typeName}");
          continue;
        }

        var extension = (Attribute.GetCustomAttribute(type, typeof(ConfigExtension), true) as ConfigExtension)?.Type ??
                        ConfigType.JSON;
        var fullName = $"{typeName}Config.{extension.ToString().ToLower()}";
        await LoaderConfig.CreateFile(fullName, type, extension);
        var obj = extension == ConfigType.JSON
          ? JsonLoader.GetJson(fullName, type)
          : TomlLoader.GetToml(fullName, type);
        _configs.Add(typeName, new ConfigWrapper(extension, obj));
      }
    }
  }
}