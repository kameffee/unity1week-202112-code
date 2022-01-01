using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Presentation.Map
{
    public interface IAreaTitlePerformer
    {
        /// <summary>
        /// 演出開始
        /// </summary>
        UniTask Perform(string titleText, CancellationToken cancellation);

        /// <summary>
        /// 演出終了
        /// </summary>
        UniTask Close(CancellationToken cancellation);
    }
}