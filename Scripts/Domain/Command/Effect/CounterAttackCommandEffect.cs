namespace Unity1week202112.Domain.Command.Effect
{
    public class CounterAttackCommandEffect : IDamageTimingEffect
    {
        public CommandEffectType EffectType => CommandEffectType.CounterAttack;
        public bool Enable { get; private set; } = true;

        public void Execute(ref Damage damage)
        {
            Enable = false;

            var newDamage = damage * 2;
            damage.AttackerStatus.Damage(newDamage);
        }
    }
}
