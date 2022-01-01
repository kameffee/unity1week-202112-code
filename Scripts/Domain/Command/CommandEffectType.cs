namespace Unity1week202112.Domain.Command
{
    /// <summary>
    /// 効果タイプ
    /// </summary>
    public enum CommandEffectType
    {
        Attack = 1, // 攻撃
        Defence = 2, // ガード
        DoubleAttack = 3, // 2回攻撃
        Heal = 4, // 回復
        CounterAttack = 5, // 反撃
        BlockSelect = 6, // 使用ロック
    }
}
