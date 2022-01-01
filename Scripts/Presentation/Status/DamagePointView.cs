using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity1week202112.Presentation.Status
{
    /// <summary>
    /// ダメージ表示
    /// </summary>
    public class DamagePointView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private TextMeshProUGUI _damege;

        public void SetDamage(int damage)
        {
            _damege.SetText($"-{damage.ToString()}");
        }

        public void PlayAnimation()
        {
            var rectTransform = transform as RectTransform;
            var sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOAnchorPosY(50f, 1f).SetRelative());
            sequence.Join(_canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.Linear).SetDelay(0.5f));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            sequence.AppendCallback(() => Destroy(gameObject));
            sequence.Play();
        }
    }
}
