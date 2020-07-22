using System;

namespace DiscordManager
{
  internal class ManagerException : Exception
  {
    public ManagerException(string message) : base(message)
    {
    }

    public ManagerException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }

  internal class ManagerArgumentException : ArgumentException
  {
    public ManagerArgumentException(string paramName) : base(paramName)
    {
    }

    public ManagerArgumentException(string paramName, string message) : base(message, paramName)
    {
    }

    public ManagerArgumentException(string paramName, string message, Exception innerException) : base(message,
      paramName, innerException)
    {
    }
  }
}