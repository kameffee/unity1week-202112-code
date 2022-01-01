using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Domain
{
    public interface IStatusUpPerformPresenter
    {
        UniTask Perform(bool isWin, StatusChange statusChange, CancellationToken cancellationToken);
    }
}
