namespace Unity1week202112.Domain.Command.Effect
{
    public class HpCommandEffect : IHpEffect
    {
        public CommandEffectType EffectType => CommandEffectType.Heal;

        public bool Enable { get; private set; } = true;

        private readonly int _healPoint;

        public HpCommandEffect(int healPoint)
        {
            _healPoint = healPoint;
        }

        public void Heal(IHealable healTarget)
        {
            Enable = false;
            healTarget.Heal(_healPoint);
        }
    }
}
