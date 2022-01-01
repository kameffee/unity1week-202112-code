using Cysharp.Threading.Tasks;

namespace Unity1week202112.Domain.Command.Effect
{
    /// <summary>
    ///  攻撃効果
    /// </summary>
    public class AttackCommandEffect : IAttackEffect
    {
        public CommandEffectType EffectType => CommandEffectType.Attack;

        public bool Enable { get; private set; } = true;

        public int Value { get; }

        public AttackCommandEffect(int value)
        {
            Value = value;
        }

        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="attacker">使用者</param>
        /// <param name="enemy">相手</param>
        public async UniTask Execute(PlayerStatusModel attacker, PlayerStatusModel enemy)
        {
            Enable = false;
            enemy.Damage(new Damage(Value, attacker));
        }
    }
}
