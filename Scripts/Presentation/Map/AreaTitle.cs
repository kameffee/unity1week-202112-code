using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity1week202112.Presentation.Map
{
    public class AreaTitle : MonoBehaviour, IAreaTitlePerformer
    {
        [SerializeField]
        private CanvasGroup _rootCanvas;

        [SerializeField]
        private TextMeshProUGUI _titleText;

        private void Awake()
        {
            _rootCanvas.alpha = 0;
            _titleText.DOFade(0, 0);
        }

        public async UniTask Perform(string titleText, CancellationToken cancellation)
        {
            _titleText.SetText(titleText);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rootCanvas.DOFade(1, 0.5f));
            sequence.AppendInterval(0.5f);
            sequence.Append(_titleText.DOFade(1, 1f));
            sequence.WithCancellation(cancellation);
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            await sequence.Play();
        }

        public async UniTask Close(CancellationToken cancellation)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_titleText.DOFade(0, 0.5f));
            sequence.AppendInterval(0.5f);
            sequence.Append(_rootCanvas.DOFade(0, 1f));
            sequence.WithCancellation(cancellation);
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            await sequence.Play();
        }
    }
}
