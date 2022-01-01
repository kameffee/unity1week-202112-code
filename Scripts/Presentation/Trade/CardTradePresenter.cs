using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.TradeCard;
using UnityEngine;

namespace Unity1week202112.Presentation.Trade
{
    /// <summary>
    /// カード交換
    /// </summary>
    public class CardTradePresenter : ITradeCardPresenter
    {
        private CardTradeWindow _window;
        private Func<Transform, TradeSelectCardModel, TradeSelectCardPresenter> _factory;
        private List<TradeSelectCardPresenter> _presenters = new List<TradeSelectCardPresenter>();

        public CardTradePresenter(
            CardTradeWindow window,
            Func<Transform, TradeSelectCardModel, TradeSelectCardPresenter> factory)
        {
            _window = window;
            _factory = factory;
        }

        /// <summary>
        /// 手に入れるカードを選ぶ
        /// </summary>
        public async UniTask<CommandCardModel> StartSelectCardPhase(IEnumerable<CommandCardModel> gettableCards,
            CancellationToken cancellation)
        {
            float delay = 0;
            var tasks = new List<UniTask<TradeSelectCardModel>>();
            var commandCardModels = gettableCards.ToList();

            foreach (var commandCardModel in commandCardModels)
            {
                var tradeCardModel = new TradeSelectCardModel(commandCardModel);
                tasks.Add(tradeCardModel.OnSelect.Select(_ => tradeCardModel).ToUniTask(true, cancellation));
                tradeCardModel.CommandCardModel.SetVisible(true);
                tradeCardModel.CommandCardModel.SetSelectable(true);
                var tradePresenter = _factory.Invoke(_window.SelectGetCardHolder, tradeCardModel);
                tradePresenter.Initialize();
                tradePresenter.PlayInAnimation(delay);

                _presenters.Add(tradePresenter);
                delay += 0.02f;
            }

            await _window.Open(cancellation);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cancellation);

            // カードを表示
            await _window.OpenGetCards(cancellation);

            // 選ばれるまで待つ
            var result = await UniTask.WhenAny(tasks);
            var selectedCommandCard = result.result;

            // 選んだカード以外の消失演出
            foreach (var commandCardModel in commandCardModels.Where(model => !model.Equals(selectedCommandCard.CommandCardModel)))
            {
                commandCardModel.Use();
            }

            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: cancellation);

            // カードを非表示
            await _window.CloseGetCards(cancellation);

            _presenters.ForEach(presenter => presenter.Dispose());
            _presenters.Clear();

            return result.result.CommandCardModel;
        }

        /// <summary>
        /// 手放すカードを選ぶ
        /// </summary>
        public async UniTask<CommandCardModel> StartReleaseSelectCardPhase(IEnumerable<CommandCardModel> releaseCards,
            CancellationToken cancellation)
        {
            var tasks = new List<UniTask<TradeSelectCardModel>>();
            float delay = 0f;
            foreach (var commandCardModel in releaseCards)
            {
                var tradeCardModel = new TradeSelectCardModel(commandCardModel);
                tasks.Add(
                    tradeCardModel.OnSelect.Select(_ => tradeCardModel).ToUniTask(true, cancellation));
                var tradePresenter = _factory.Invoke(_window.SelectReleaseCardHolder, tradeCardModel);
                tradeCardModel.CommandCardModel.SetVisible(true);
                tradeCardModel.CommandCardModel.SetSelectable(true);
                tradeCardModel.CommandCardModel.SetSelected(false);
                tradePresenter.Initialize();
                tradePresenter.PlayInAnimation(delay);

                _presenters.Add(tradePresenter);
                delay += 0.02f;
            }

            // 手放すカードを表示
            await _window.OpenReleaseCards(cancellation);

            // 選ばれるまで待つ
            var selectedCard = await UniTask.WhenAny(tasks);

            // 選んだカードの消失演出
            selectedCard.result.CommandCardModel.Use();
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: cancellation);

            // 手放すカードを非表示
            await _window.CloseReleaseCards(cancellation);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: cancellation);
            await _window.Close(cancellation);

            _presenters.ForEach(presenter => presenter.Dispose());
            _presenters.Clear();

            return selectedCard.result.CommandCardModel;
        }
    }
}
