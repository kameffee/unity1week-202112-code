using System;
using UniRx;
using Unity1week202112.Domain.Command;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// プレイヤーの状態
    /// </summary>
    public class PlayerStatusModel : IHealable, IDamageObservable
    {
        public int MaxHp { get; }

        public IReadOnlyReactiveProperty<int> Hp => _hp;
        private readonly ReactiveProperty<int> _hp = new ReactiveProperty<int>();

        // 回復通知
        public IObservable<HpHeal> OnHeal => _onHeal;
        private readonly Subject<HpHeal> _onHeal = new Subject<HpHeal>();

        public IObservable<HpDamage> OnDamage => _onDamage;
        private readonly Subject<HpDamage> _onDamage = new Subject<HpDamage>();

        // バフ追加時
        public IObservable<ICommandEffect> OnAddBuff => _onAddBuff;
        private readonly Subject<ICommandEffect> _onAddBuff = new Subject<ICommandEffect>();

        public IObservable<ICommandEffect> OnRemoveBuff => _onRemoveBuff;
        private readonly Subject<ICommandEffect> _onRemoveBuff = new Subject<ICommandEffect>();

        private ReactiveProperty<IDamageTimingEffect> _damageTimingEffect = new ReactiveProperty<IDamageTimingEffect>();

        public PlayerStatusModel(int hp)
        {
            MaxHp = hp;
            _hp.Value = hp;
        }

        public void SetHp(int hp)
        {
            _hp.Value = hp;
        }

        public void FullHp()
        {
            SetHp(MaxHp);
        }

        public void Damage(Damage damage)
        {
            if (_damageTimingEffect.Value != null)
            {
                if (_damageTimingEffect.Value.Enable)
                {
                    _damageTimingEffect.Value.Execute(ref damage);
                }

                _onRemoveBuff.OnNext(_damageTimingEffect.Value);

                // 破棄
                _damageTimingEffect.Value = null;
            }

            var hpDamage = new HpDamage(_hp.Value, damage.Value);
            _hp.Value = hpDamage.ToHp;
            _onDamage.OnNext(hpDamage);
        }

        public bool IsDead()
        {
            return _hp.Value <= 0;
        }

        /// <summary>
        /// ダメージタイミングの効果を付与
        /// </summary>
        /// <param name="effect"></param>
        public void AddDamageTimingEffect(IDamageTimingEffect effect)
        {
            _damageTimingEffect.Value = effect;
            _onAddBuff.OnNext(effect);
        }

        public void Heal(int heal)
        {
            var hpHeal = new HpHeal(_hp.Value, MaxHp, heal);
            _hp.Value = hpHeal.AfterHp;
            _onHeal.OnNext(hpHeal);
        }
    }

    public interface IDamageObservable
    {
        IObservable<HpDamage> OnDamage { get; }
    }
}
