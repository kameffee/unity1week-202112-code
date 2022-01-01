using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202112.Presentation.Status
{
    public class BuffView : MonoBehaviour, IBuffStatusView
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private Image _icon;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
        }
        

        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public async UniTask PlayInAnimation()
        {
            _canvasGroup.DOFade(1, 0.2f);
        }

        public async UniTask PlayOutAnimation()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0, 1f).SetEase(Ease.Linear));
            sequence.Join(_icon.rectTransform.DOScale(1.5f, 1f));
            sequence.Append(_icon.rectTransform.DOScale(1, 0f));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            await sequence.Play();
        }
    }
}
