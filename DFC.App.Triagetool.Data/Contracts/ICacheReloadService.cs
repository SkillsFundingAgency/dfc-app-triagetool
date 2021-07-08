using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Data.Contracts
{
    public interface ICacheReloadService
    {
        Task Reload(CancellationToken stoppingToken);
    }
}
