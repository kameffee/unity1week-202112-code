using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Presentation
{
    public interface IEndingViewPerformer
    {
        UniTask Open(CancellationToken cancellation);
    }
}