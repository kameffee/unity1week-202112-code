using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Scene;
using VContainer.Unity;

namespace Unity1week202112.Presentation.ScreenFade
{
    /// <summary>
    /// 画面フェード Presenter
    /// </summary>
    public sealed class ScreenFadePresenter : IInitializable, IDisposable
    {
        private readonly IScreenFadeHandler _handler;
        private readonly IScreenFadeView _view;

        private readonly CancellationTokenSource _tokenSource;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public ScreenFadePresenter(IScreenFadeHandler handler, IScreenFadeView view)
        {
            _handler = handler;
            _view = view;
            _tokenSource = new CancellationTokenSource();
        }

        public void Initialize()
        {
            // フェードアウト
            _handler.OnFadeOut
                .Subscribe(duration => FadeOut(duration).Forget())
                .AddTo(_disposable);

            // フェードイン
            _handler.OnFadeIn
                .Subscribe(duration => FadeIn(duration).Forget())
                .AddTo(_disposable);
        }

        private async UniTaskVoid FadeOut(float duration)
        {
            await _view.FadeOut(duration, _tokenSource.Token);
            _handler.OnFadeOutComplete();
        }

        private async UniTaskVoid FadeIn(float duration)
        {
            await _view.FadeIn(duration, _tokenSource.Token);
            _handler.OnFadeInComplete();
        }

        public void Dispose()
        {
            _tokenSource?.Dispose();
            _disposable?.Dispose();
        }
    }
}
