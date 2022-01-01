using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202112.Presentation.ScreenFade
{
    /// <summary>
    /// 画面フェード View
    /// </summary>
    public sealed class ScreenFadeView : MonoBehaviour, IScreenFadeView
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;
        }

        public async UniTask FadeOut(float duration, CancellationToken cancellationToken)
        {
            _canvasGroup.blocksRaycasts = true;
            await _canvasGroup.DOFade(1, duration).WithCancellation(cancellationToken);
        }

        public async UniTask FadeIn(float duration, CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(0, duration).WithCancellation(cancellationToken);
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
