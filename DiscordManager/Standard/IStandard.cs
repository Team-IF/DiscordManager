using System.Threading.Tasks;
using DiscordManager.Command;

namespace DiscordManager.Standard
{
  public interface IStandard<in T>
  {
    Task<bool> CheckAsync(Context context, T param);
  }
}