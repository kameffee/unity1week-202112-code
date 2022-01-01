using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Unity1week202112.Domain.Scene
{
    /// <summary>
    /// 画面を覆うフェード
    /// </summary>
    public sealed class ScreenFader : IScreenFadeHandler, IScreenFader, IDisposable
    {
        public IReadOnlyReactiveProperty<bool> IsOut => _isOut;
        private readonly ReactiveProperty<bool> _isOut = new ReactiveProperty<bool>();

        // なにかしらのフェード中
        public IReadOnlyReactiveProperty<bool> IsFading => _isFading;
        private readonly ReactiveProperty<bool> _isFading = new ReactiveProperty<bool>();

        public IObservable<float> OnFadeOut => _onFadeOut;
        private readonly Subject<float> _onFadeOut = new Subject<float>();

        public IObservable<float> OnFadeIn => _onFadeIn;
        private readonly Subject<float> _onFadeIn = new Subject<float>();

        public IObservable<Unit> OnFadeEnd => _onFadeEnd;
        private readonly Subject<Unit> _onFadeEnd = new Subject<Unit>();

        /// <summary>
        /// 画面を覆う
        /// </summary>
        public async UniTask FadeOut(float duration = 0.5f, CancellationToken cancellationToken = default)
        {
            // すでに覆っている
            if (_isOut.Value) return;

            _isFading.Value = true;

            _isOut.Value = true;
            _onFadeOut.OnNext(duration);
            await _onFadeEnd.ToUniTask(true, cancellationToken);

            _isFading.Value = false;
        }

        /// <summary>
        /// 画面を表示
        /// </summary>
        public async UniTask FadeIn(float duration = 0.5f, CancellationToken cancellationToken = default)
        {
            // すでに表示してある.
            if (!_isOut.Value) return;

            _isFading.Value = true;

            _isOut.Value = false;
            _onFadeIn.OnNext(duration);
            await _onFadeEnd.ToUniTask(true, cancellationToken);

            _isFading.Value = false;
        }

        public void OnFadeOutComplete() => _onFadeEnd.OnNext(Unit.Default);

        public void OnFadeInComplete() => _onFadeEnd.OnNext(Unit.Default);

        public void Dispose()
        {
            _isOut?.Dispose();
            _onFadeOut?.Dispose();
            _onFadeIn?.Dispose();
        }
    }
}
