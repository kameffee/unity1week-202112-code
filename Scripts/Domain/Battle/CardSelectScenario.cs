using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity1week202112.Domain.Command;
using UnityEngine;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// カード選択
    /// </summary>
    public sealed class CardSelectScenario : IWaitCardSelect
    {
        public IObservable<CommandCardModel> OnSelectCard => _onSelectCard;
        private Subject<CommandCardModel> _onSelectCard = new Subject<CommandCardModel>();

        public async UniTask<CommandCardModel> WaitPlayerSelectCard()
        {
            Debug.Log("プレイヤーの入力待ち Start");

            // プレイヤーのアクション選択
            var result = await _onSelectCard.ToUniTask(true);

            Debug.Log("プレイヤーの入力待ち End");
            return result;
        }

        /// <summary>
        /// カード選択
        /// </summary>
        /// <param name="cardModel">選んだカード</param>
        public void Select(CommandCardModel cardModel)
        {
            _onSelectCard.OnNext(cardModel);
        }
    }
}
