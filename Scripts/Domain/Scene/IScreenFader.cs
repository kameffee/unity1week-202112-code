using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Domain.Scene
{
    public interface IScreenFader
    {
        UniTask FadeOut(float duration = 0.5f, CancellationToken cancellationToken = default);
        UniTask FadeIn(float duration = 0.5f, CancellationToken cancellationToken = default);
    }
}
