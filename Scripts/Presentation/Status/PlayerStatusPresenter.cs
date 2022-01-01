using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202112.Domain;
using Unity1week202112.Domain.Command;
using VContainer.Unity;

namespace Unity1week202112.Presentation.Status
{
    public class PlayerStatusPresenter : IStartable, IDisposable
    {
        private readonly PlayerStatusModel _model;
        private readonly PlayerHpView _hpView;
        private readonly IBuffStatusView _buffStatus;
        private readonly CommandEffectIconProvider _effectIconProvider;
        private readonly IDamageFeedbackView _damageFeedback;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public PlayerStatusPresenter(
            IBattlePlayer model,
            PlayerHpView hpView,
            IBuffStatusView buffStatus,
            CommandEffectIconProvider effectIconProvider,
            IDamageFeedbackView damageFeedback)
        {
            _model = model.Status;
            _hpView = hpView;
            _buffStatus = buffStatus;
            _effectIconProvider = effectIconProvider;
            _damageFeedback = damageFeedback;
        }

        public void Start()
        {
            _hpView.SetMaxHp(_model.MaxHp);
            _hpView.RenderHp(_model.Hp.Value);

            // ダメージを受けた時
            _model.OnDamage
                .Subscribe(hpDamage => _hpView.Damege(hpDamage))
                .AddTo(_disposable);

            _model.OnDamage
                .Where(damage => damage.Damage > 0)
                .Subscribe(_ => _damageFeedback.Damage().Forget())
                .AddTo(_disposable);

            // 回復時
            _model.OnHeal
                .Subscribe(heal => _hpView.Cure(heal))
                .AddTo(_disposable);

            _model.OnAddBuff
                .Subscribe(async effect =>
                {
                    _buffStatus.SetIcon(await _effectIconProvider.GetIcon((int)effect.EffectType));
                    _buffStatus.PlayInAnimation();
                })
                .AddTo(_disposable);

            _model.OnRemoveBuff
                .Subscribe(async effect => await _buffStatus.PlayOutAnimation())
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
