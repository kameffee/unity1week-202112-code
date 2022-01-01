using System.Linq;
using Unity1week202112.Domain.Extensions;

namespace Unity1week202112.Domain.Command.Effect
{
    /// <summary>
    /// 相手の一部コマンドカードを使用不可にする効果
    /// </summary>
    public class UseLockDebuffCommandEffect : IFieldCardsEffect
    {
        public CommandEffectType EffectType => CommandEffectType.BlockSelect;
        public bool Enable { get; private set; } = true;

        private readonly int _value;

        public UseLockDebuffCommandEffect(int value)
        {
            _value = value;
        }

        public void Execute(FieldCardsModel myFieldCards, FieldCardsModel otherFieldCards)
        {
            // 選択可能カード中からランダムに一枚選ぶ
            var selectableCards = otherFieldCards.GetSelectableCards().Shuffle().ToArray();
            if (selectableCards.Any())
            {
                var target = selectableCards.First();
                target.LockUse(_value);
            }
            
            Enable = false;
        }
    }
}
