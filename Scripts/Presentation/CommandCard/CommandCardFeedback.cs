using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Kameffee.AudioPlayer;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Unity1week202112.Presentation.CommandCard
{
    public class CommandCardFeedback : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _root;

        [Header("Audio")]
        [SerializeField]
        private AudioClip _hoverClip;

        public bool IsEnable { get; private set; }

        private Tween _tween;

        private ObservablePointerEnterTrigger _enterTrigger;
        private ObservablePointerExitTrigger _exitTrigger;
        private bool mouthOver;

        private void Awake()
        {
            _enterTrigger = gameObject.AddComponent<ObservablePointerEnterTrigger>();
            _exitTrigger = gameObject.AddComponent<ObservablePointerExitTrigger>();

            _enterTrigger.OnPointerEnterAsObservable()
                .Subscribe(_ => mouthOver = true)
                .AddTo(this);

            _exitTrigger.OnPointerExitAsObservable()
                .Subscribe(_ => mouthOver = false)
                .AddTo(this);

            _enterTrigger.OnPointerEnterAsObservable()
                .Subscribe(data => { OnPointerEnter(this.GetCancellationTokenOnDestroy()).Forget(); })
                .AddTo(this);
        }

        public async UniTaskVoid OnPointerEnter(CancellationToken cancellation)
        {
            if (!IsEnable) return;

            PlayEnterAnimation();

            if (mouthOver)
            {
                // 抜けるまで
                await gameObject.GetAsyncPointerExitTrigger().OnPointerExitAsync(cancellation);
            }

            PlayExitAnimation();
        }

        private void PlayEnterAnimation()
        {
            AudioPlayer.Instance.Se.Play(_hoverClip);
            _tween?.Kill();

            _root.DOScale(1.1f, 0.1f);
            _root.DOLocalRotate(Vector3.forward * -1f, 0);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_root.DOLocalRotate(Vector3.forward * 1f, 0.03f).SetEase(Ease.Linear));
            sequence.Append(_root.DOLocalRotate(Vector3.forward * -1f, 0.03f).SetEase(Ease.Linear));
            sequence.SetLoops(-1, LoopType.Yoyo);
            _tween = sequence.Play();
        }

        private void PlayExitAnimation()
        {
            _tween?.Kill();
            _root.DOScale(1f, 0.1f);
            _root.DOLocalRotate(Vector3.zero, 0);
        }

        private void UpdateState()
        {
            if (mouthOver && IsEnable)
            {
                OnPointerEnter(this.GetCancellationTokenOnDestroy()).Forget();
            }
            else
            {
                PlayExitAnimation();
            }
        }

        public void SetEnable(bool enable)
        {
            IsEnable = enable;
            UpdateState();
        }
    }
}
