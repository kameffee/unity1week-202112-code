namespace Unity1week202112.Domain.Command
{
    public interface ICommandEffect
    {
        CommandEffectType EffectType { get; }

        public bool Enable { get; }
    }
}
