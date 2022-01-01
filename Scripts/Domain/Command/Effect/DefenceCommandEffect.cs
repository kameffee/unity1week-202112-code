namespace Unity1week202112.Domain.Command.Effect
{
    public class DefenceCommandEffect : IDamageTimingEffect, ICommandEffect
    {
        public CommandEffectType EffectType => CommandEffectType.Defence;
        public bool Enable { get; private set; } = true;

        public void Execute(ref Damage damege)
        {
            Enable = false;
            damege = new Damage(0, damege.AttackerStatus);;
        }
    }
}
