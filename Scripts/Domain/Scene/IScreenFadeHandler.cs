using System;
using UniRx;

namespace Unity1week202112.Domain.Scene
{
    public interface IScreenFadeHandler
    {
        IReadOnlyReactiveProperty<bool> IsOut { get; }
        IReadOnlyReactiveProperty<bool> IsFading { get; }
        IObservable<float> OnFadeOut { get; }
        IObservable<float> OnFadeIn { get; }
        IObservable<Unit> OnFadeEnd { get; }
        void OnFadeOutComplete();
        void OnFadeInComplete();
    }
}