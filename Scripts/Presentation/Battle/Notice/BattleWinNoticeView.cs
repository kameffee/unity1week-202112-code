using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity1week202112.Presentation
{
    /// <summary>
    /// バトル勝利演出
    /// </summary>
    public class BattleWinNoticeView : MonoBehaviour, IBattleWinPerformer
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
            _textRoot.GetComponent<TextMeshProUGUI>().DOFade(0, 0);
        }

        public async UniTask Perform(CancellationToken cancellation)
        {
            Initialize();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, 0.2f));
            sequence.AppendInterval(0.2f);
            sequence.Append(_textRoot.GetComponent<TextMeshProUGUI>().DOFade(0, 0f));
            sequence.Append(_textRoot.GetComponent<TextMeshProUGUI>().DOFade(1, 0.8f).SetEase(Ease.Flash, 11));
            sequence.AppendInterval(1f);
            sequence.Append(_canvasGroup.DOFade(0, 0.6f));

            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            await sequence.Play();
        }
    }
}
