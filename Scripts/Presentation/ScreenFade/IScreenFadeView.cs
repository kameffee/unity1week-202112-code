using System.Threading;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Presentation.ScreenFade
{
    public interface IScreenFadeView
    {
        UniTask FadeOut(float duration, CancellationToken cancellationToken);

        UniTask FadeIn(float duration, CancellationToken cancellationToken);
    }
}
