using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unity1week202112.Presentation.UI
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
        IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private Graphic _graphic;

        [SerializeField]
        private Color _normalColor = new Color(1, 1, 1, 1);

        [SerializeField]
        private Color _pressColor = new Color(0.8f, 0.8f, 0.8f);

        [SerializeField]
        private float _downDurationTime = 0.15f;

        [SerializeField]
        private float _upDurationTime = 0.2f;

        [SerializeField]
        private float _pressScaleRatio = 0.95f;

        public Button.ButtonClickedEvent OnClick => _onClick;
        private Button.ButtonClickedEvent _onClick = new Button.ButtonClickedEvent();

        private RectTransform _rectTransform;

        private Tween _tween;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_tween != null && _tween.IsPlaying())
            {
                _tween.Kill();
            }

            PlayPressAnimation();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_tween != null && _tween.IsPlaying())
            {
                _tween.Kill();
            }

            PlayReleaseAnimation();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick.Invoke();
        }

        private void PlayPressAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_graphic.DOColor(_pressColor, _downDurationTime));
            sequence.Join(_rectTransform.DOScale(_pressScaleRatio, _downDurationTime).SetEase(Ease.OutSine));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            _tween = sequence.Play();
        }

        private void PlayReleaseAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_graphic.DOColor(_normalColor, _upDurationTime));
            sequence.Join(_rectTransform.DOScale(1f, _upDurationTime).SetEase(Ease.OutSine));
            sequence.WithCancellation(this.GetCancellationTokenOnDestroy());
            _tween = sequence.Play();
        }
    }

    public static class UIComponentExtensions
    {
        public static IObservable<Unit> OnClickAsObservable(this CustomButton button)
        {
            return button.OnClick.AsObservable();
        }
    }
}
