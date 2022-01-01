using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Presentation
{
    public interface IBattleStartPerformer
    {
        UniTask Perform(CancellationToken cancellation);
    }
}