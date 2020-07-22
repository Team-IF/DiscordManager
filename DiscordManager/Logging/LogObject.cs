using System;
using System.Text;

namespace DiscordManager.Logging
{
  public struct LogObject
  {
    public LogLevel Level { get; }
    public string Source { get; }
    public string Message { get; }
    public Exception Exception { get; }

    internal LogObject(LogLevel level, string source, string message, Exception exception = null)
    {
      Level = level;
      Source = source;
      Message = message;
      Exception = exception;
    }

    public override string ToString()
    {
      return ToString();
    }

    public string ToString(bool fullException = true)
    {
      var exMessage = fullException ? Exception?.ToString() : Exception?.Message;
      var maxLength = 1 + (Message?.Length ?? 0) + 1 +
                      (Source?.Length ?? 0) + 1 +
                      (exMessage?.Length ?? 0) + 1;
      var builder = new StringBuilder(maxLength);
      if (Source != null)
      {
        builder.Append(Source);
        builder.Append(' ');
      }

      builder.Append(Level);
      builder.Append(' ');
      if (!string.IsNullOrEmpty(Message))
        for (var i = 0; i < Message.Length; i++)
        {
          //Strip control chars
          var c = Message[i];
          if (!char.IsControl(c))
            builder.Append(c);
        }

      if (exMessage != null)
      {
        if (!string.IsNullOrEmpty(Message))
        {
          builder.Append(" : ");
          builder.AppendLine();
        }

        builder.Append(exMessage);
      }

      return builder.ToString();
    }
  }
}