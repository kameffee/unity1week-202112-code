namespace Unity1week202112.Domain.Command
{
    /// <summary>
    /// ダメージを受ける時に出る効果
    /// </summary>
    public interface IDamageTimingEffect : ICommandEffect
    {
        void Execute(ref Damage damege);
    }
}
