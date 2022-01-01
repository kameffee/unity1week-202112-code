using System;
using Cysharp.Threading.Tasks;

namespace Unity1week202112.Domain.Command.Effect
{
    /// <summary>
    /// 2回攻撃
    /// </summary>
    public class DoubleAttackCommandEffect : IAttackEffect
    {
        public CommandEffectType EffectType => CommandEffectType.DoubleAttack;
        public bool Enable { get; private set; } = true;

        private readonly int _damage;

        public DoubleAttackCommandEffect(int damage)
        {
            _damage = damage;
        }

        public async UniTask Execute(PlayerStatusModel attacker, PlayerStatusModel enemy)
        {
            Enable = false;
            
            enemy.Damage(new Damage(_damage, attacker));

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            enemy.Damage(new Damage(_damage, attacker));
        }
    }
}
