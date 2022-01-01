using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain.Command;

namespace Unity1week202112.Domain
{
    public interface IBattlePlayer
    {
        PlayerStatusModel Status { get; }

        FieldCardsModel FieldCards { get; }

        void BattlePrepare();
        UniTask TurnBegin();

        UniTask InitialCardDraw(CancellationToken cancellation);
        
        /// <summary>
        /// コマンドカードを引く
        /// </summary>
        /// <param name="cancellation"></param>
        UniTask CardDraw(CancellationToken cancellation);

        /// <summary>
        /// 選ぶカードがあるか
        /// </summary>
        bool Selectable();

        /// <summary>
        /// コマンドの選択
        /// </summary>
        /// <returns></returns>
        UniTask<CommandCardModel> SelectCommand();

        /// <summary>
        /// コマンドを使う
        /// </summary>
        /// <param name="useCard"></param>
        /// <param name="otherPlayer"></param>
        UniTask UseCommand(CommandCardModel useCard, IBattlePlayer otherPlayer);

        /// <summary>
        /// フィールドのカードが空か
        /// </summary>
        /// <returns></returns>
        bool IsEmptyHandsCard();
        UniTask ReInitializeDraw(CancellationToken cancellation);
    }
}
