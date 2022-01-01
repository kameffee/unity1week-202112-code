using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Presentation
{
    /// <summary>
    /// バトル敗北演出
    /// </summary>
    public interface IBattleLosePerformer
    {
        UniTask Perform(CancellationToken cancellation);
    }
}
