using System.Collections.Generic;
using Discord;

namespace DiscordManager.Voice
{
  internal class VoiceManager
  {
    private static DiscordManager? _manager { get; set; }
    private readonly Dictionary<ulong, ulong> _connectedChannels; // guild id, voice channel id
    public VoiceManager()
    {
      _manager = DiscordManager.Manager;
      _connectedChannels = new Dictionary<ulong, ulong>();
    }

    static VoiceManager()
    {
      ThrowIfManagerIsNull();
    }

    private static void ThrowIfManagerIsNull()
    {
      Checker.NotNull(_manager, nameof(_manager), "Not Allow use VoiceManager, if you want use VoiceManager add WithVoiceManager() at DiscordBuilder");
    }

    public void Connect(IGuild guild, IVoiceChannel voiceChannel)
    {
      voiceChannel.ConnectAsync();
      _connectedChannels.Add(guild.Id, voiceChannel.Id);
    }
    
    public void Disconnect(IGuild guild)
    {
      if (_connectedChannels.TryGetValue(guild.Id, out var channelId))
      {
        var voiceChannel = guild.GetVoiceChannelAsync(channelId).Result;
        voiceChannel.DisconnectAsync();
        _connectedChannels.Remove(guild.Id);
      }
    }

    public bool ExistVoiceChannel(IGuild guild) => _connectedChannels.ContainsKey(guild.Id);
  }
}