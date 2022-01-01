using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Unity1week202112.Presentation
{
    public class BattleLoseNoticeView : MonoBehaviour, IBattleLosePerformer
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private RectTransform _textRoot;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            _canvasGroup.alpha = 0;
            _textRoot.DOAnchorPosY(-_textRoot.sizeDelta.y, 0);
        }

        public async UniTask Perform(CancellationToken cancellation)
        {
            Initialize();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, 0.2f));
            sequence.Append(_textRoot.DOAnchorPosY(0, 0.5f));
            sequence.AppendInterval(1f);
            sequence.Append(_canvasGroup.DOFade(0, 0.8f));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            await sequence.Play();
        }
    }
}
