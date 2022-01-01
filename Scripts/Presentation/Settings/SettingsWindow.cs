using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Unity1week202112.Presentation
{
    public class SettingsWindow : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Image _image;

        public IObservable<Unit> OnClickClose => _image.OnPointerClickAsObservable().AsUnitObservable();

        private void Awake()
        {
            _canvasGroup.alpha = 0;
        }

        public async UniTask Open(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(1, 0.2f)
                .WithCancellation(cancellationToken)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public async UniTask Close(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(0, 0.2f)
                .WithCancellation(cancellationToken)
                .WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
