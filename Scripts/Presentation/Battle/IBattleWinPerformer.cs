using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Presentation
{
    public interface IBattleWinPerformer
    {
        UniTask Perform(CancellationToken cancellation);
    }
}