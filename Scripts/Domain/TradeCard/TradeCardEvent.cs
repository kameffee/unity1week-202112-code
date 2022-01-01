using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Deck;
using Unity1week202112.Domain.Lottery;
using Unity1week202112.Domain.Map;

namespace Unity1week202112.Domain.TradeCard
{
    /// <summary>
    /// カード交換イベント
    /// </summary>
    public class TradeCardEvent
    {
        private readonly LotteryCard _lotteryCard;
        private readonly ITradeCardPresenter _tradeCardPresenter;
        private readonly PlayerDeckRepository _playerDeckRepository;
        private readonly CommandCardConverter _converter = new CommandCardConverter();

        public TradeCardEvent(
            ITradeCardPresenter tradeCardPresenter,
            PlayerDeckRepository playerDeckRepository,
            LotteryCard lotteryCard)
        {
            _tradeCardPresenter = tradeCardPresenter;
            _playerDeckRepository = playerDeckRepository;
            _lotteryCard = lotteryCard;
        }

        public async UniTask Execute(MapPoint mapPoint, CancellationToken cancellation)
        {
            CommandDeckEntity deckEntity = _playerDeckRepository.Load();
            IEnumerable<CommandCardModel> commandCardModels = _converter.Create(deckEntity.CardList);

            // 交換して手に入るカードを抽選する
            List<CommandCardEntity> lotteryCards = _lotteryCard.Lottery(mapPoint.PointId).ToList();

            // Modelへ変換
            IEnumerable<CommandCardModel> gettableCards = lotteryCards
                .Select(entity => _converter.ToModel(entity));

            // 手に入れるカードを選ぶ
            CommandCardModel getCard = await _tradeCardPresenter.StartSelectCardPhase(gettableCards, cancellation);

            // 捨てるカードを選ぶ
            CommandCardModel releaseCard = await _tradeCardPresenter.StartReleaseSelectCardPhase(commandCardModels, cancellation);

            // 保存する
            deckEntity.Update(releaseCard.Id, _converter.ToEntity(getCard));
            _playerDeckRepository.Save(deckEntity);
        }
    }
}
