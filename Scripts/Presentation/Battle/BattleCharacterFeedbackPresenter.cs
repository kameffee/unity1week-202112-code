using System;
using UniRx;
using Unity1week202112.Domain;
using VContainer.Unity;

namespace Unity1week202112.Presentation
{
    /// <summary>
    /// ダメージイベントの橋渡し
    /// </summary>
    public class BattleCharacterFeedbackPresenter : IInitializable, IDisposable
    {
        private readonly IDamageObservable _damageObservable;
        private IDamageFeedbackView _damageFeedbackView;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public BattleCharacterFeedbackPresenter(
            IDamageObservable damageObservable,
            IDamageFeedbackView damageFeedbackView)
        {
            _damageObservable = damageObservable;
            _damageFeedbackView = damageFeedbackView;
        }

        public void Initialize()
        {
            _damageObservable.OnDamage
                .Subscribe(hpDamage => _damageFeedbackView.Damage())
                .AddTo(_disposable);
        }

        public void Dispose() => _disposable?.Dispose();
    }
}
