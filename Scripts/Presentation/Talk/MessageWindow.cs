using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity1week202112.Presentation.Talk
{
    public class MessageWindow : MonoBehaviour, IMessageWindow
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _messageText;

        [SerializeField]
        private float _textSpeed = 0.1f;

        private RectTransform rectTransform;

        private Tween _tween;
        private Tween _shakeTween;

        private void Awake()
        {
            _canvasGroup.alpha = 0;
            rectTransform = transform as RectTransform;
            _messageText.text = "";
        }

        public async UniTask Open()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            _canvasGroup.DOFade(1, 0.3f);
        }

        public async UniTask Close()
        {
            _tween?.Kill();

            await _canvasGroup.DOFade(0, 0.5f);

            // Reset
            _shakeTween?.Kill();
            this._messageText.text = "";

            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public async UniTask StartMessage(string message)
        {
            _tween?.Kill();
            _shakeTween?.Kill();

            _messageText.text = message;

            DOTweenTMPAnimator tmproAnimator = new DOTweenTMPAnimator(_messageText);

            Sequence shake = DOTween.Sequence();
            Sequence sequence = DOTween.Sequence();
            // 
            for (int i = 0; i < tmproAnimator.textInfo.characterCount; ++i)
            {
                // 透明化
                sequence.Prepend(tmproAnimator.DOFadeChar(i, 0f, 0));
                // 登場開始アニメ
                sequence.Join(DOTween.Sequence()
                    .Append(tmproAnimator.DOFadeChar(i, 1, 0.2f))
                    .SetDelay(0.08f * i).Play());

                // 揺らす
                shake.Join(tmproAnimator.DOShakeCharOffset(i, 1f, 1, 2, Random.Range(0f, 180f))
                    .SetEase(Ease.Linear));
            }

            shake.SetLoops(-1, LoopType.Yoyo);

            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());

            _tween = sequence;
            _shakeTween = shake;

            shake.Play();

            // シーケンスが終わる or 入力がある まで待つ
            var sequenceWait = sequence.Play().ToUniTask();
            var input = UniTask.WaitUntil(() => Input.anyKeyDown || Input.GetMouseButtonDown(0));
            await UniTask.WhenAny(sequenceWait, input);

            // 完了状態にする
            _tween.Complete();
            _tween = null;
        }

        public void SetMessage(string message)
        {
            _messageText.text = message;
        }
    }
}
