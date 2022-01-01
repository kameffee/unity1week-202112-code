using Unity1week202112.Presentation;

namespace Unity1week202112.Domain.Scene
{
    /// <summary>
    /// バトル結果
    /// </summary>
    public class BattleResultParameter : ISceneParameter
    {
        /// <summary>
        /// 勝敗
        /// </summary>
        public BattleCondition BattleCondition { get; }

        public BattleResultParameter(BattleCondition battleCondition)
        {
            BattleCondition = battleCondition;
        }
    }
}
