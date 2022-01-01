using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Unity1week202112.Domain.Command
{
    /// <summary>
    /// フィールド上に出ているカード
    /// </summary>
    public sealed class FieldCardsModel
    {
        public IReadOnlyList<CommandCardModel> TopCards => _allCards[0];
        public IReadOnlyList<CommandCardModel> BottomCards => _allCards[1];

        public IObservable<CommandCardModel> OnAdd => _onAdd;
        private readonly Subject<CommandCardModel> _onAdd = new Subject<CommandCardModel>();

        public IObservable<CommandCardModel> OnUse => _onUse;
        private readonly Subject<CommandCardModel> _onUse = new Subject<CommandCardModel>();

        private readonly Dictionary<int, List<CommandCardModel>> _allCards = new Dictionary<int, List<CommandCardModel>>()
        {
            { 0, new List<CommandCardModel>() },
            { 1, new List<CommandCardModel>() },
        };

        public FieldCardsModel()
        {
        }

        public bool IsEmpty()
        {
            return !_allCards.SelectMany(pair => pair.Value).Any();
        }

        /// <summary>
        /// 選択可能なカード群
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CommandCardModel> GetSelectableCards()
        {
            var selectableCards = new List<CommandCardModel>();
            foreach (var commandCardModels in _allCards.Values)
            {
                // 先頭2枚ずつ
                selectableCards.AddRange(commandCardModels.Take(2)
                    .Where(model => model.LockUseStatus.Value.Finished)
                    .Where(model => !model.Selected.Value));
                
            }

            return selectableCards;
        }

        /// <summary>
        /// カードの追加
        /// </summary>
        /// <param name="cardModel"></param>
        /// <param name="row">追加する列</param>
        public void AddCard(CommandCardModel cardModel, int row)
        {
            List<CommandCardModel> columnCardList = _allCards[row];
            cardModel.SetPosition(new Vector2Int(columnCardList.Count, row));
            columnCardList.Add(cardModel);

            _onAdd.OnNext(cardModel);
        }

        /// <summary>
        /// 削除する
        /// </summary>
        /// <param name="cardModel"></param>
        public void RemoveCard(CommandCardModel cardModel)
        {
            var targetList = _allCards[cardModel.Position.Value.y];
            targetList.Remove(cardModel);

            // スライド
            for (var row = 0; row < _allCards.Values.Count; row++)
            {
                var rowList = _allCards[row];
                for (var column = 0; column < rowList.Count; column++)
                {
                    rowList[column].SetPosition(new Vector2Int(column, row));
                }
            }
        }

        public void UpdateCardsStatus()
        {
            for (var row = 0; row < _allCards.Values.Count; row++)
            {
                var rowList = _allCards[row];
                // 先頭3枚は見える
                foreach (var commandCardModel in rowList.Take(3))
                {
                    commandCardModel.SetVisible(true);
                }
                
                // 先頭2枚目のみ選択可能
                foreach (var commandCardModel in rowList.Take(2))
                {
                    commandCardModel.SetSelectable(true);
                }
            }
        }

        public void TurnBegin()
        {
            foreach (var card in _allCards.Values.SelectMany(list => list))
            {
                card.CountTurn();
            }
        }
    }
}
