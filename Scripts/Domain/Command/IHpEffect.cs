namespace Unity1week202112.Domain.Command
{
    /// <summary>
    /// HP操作効果
    /// </summary>
    public interface IHpEffect : ICommandEffect
    {
        void Heal(IHealable healTarget);
    }
}
