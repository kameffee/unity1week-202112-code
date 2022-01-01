using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202112.Presentation.EffectDetail
{
    /// <summary>
    /// 説明ウィンドウ
    /// </summary>
    public class CommandEffectDetailView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _dim;

        [SerializeField]
        private Button _backgroundButton;

        [SerializeField]
        private CanvasGroup _windowRoot;

        [SerializeField]
        private RectTransform _root;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _effectName;

        [SerializeField]
        private TextMeshProUGUI _description;

        public IObservable<Unit> OnClickBackground => _backgroundButton.OnClickAsObservable();

        public IObservable<Unit> OnCloseEvent => this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(1));

        private void Awake()
        {
            _windowRoot.alpha = 0;
            _dim.alpha = 0;
        }

        public void SetImage(Sprite icon) => _icon.sprite = icon;

        public void SetEffectName(string effectName) => _effectName.SetText(effectName);

        public void SetDescription(string desc) => _description.SetText(desc);

        public async UniTask Open()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_root.DOScale(0.9f, 0f));
            sequence.Append(_dim.DOFade(1, 0.16f).SetEase(Ease.Linear));
            sequence.Join(_windowRoot.DOFade(1, 0.16f).SetEase(Ease.Linear));
            sequence.Join(_root.DOScale(1, 0.16f));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            await sequence.Play();
        }

        public async UniTask Close()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_dim.DOFade(0f, 0.16f).SetEase(Ease.Linear));
            sequence.Join(_windowRoot.DOFade(0f, 0.16f).SetEase(Ease.Linear));
            sequence.Join(_root.DOScale(0.9f, 0.16f));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            await sequence.Play();
            Destroy(gameObject);
        }
    }
}
