using System;
using System.Collections.Generic;
using System.Linq;
using Unity1week202112.Data.Master;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Deck;
using Unity1week202112.Domain.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity1week202112.Domain.Lottery
{
    /// <summary>
    /// カードを抽選する
    /// </summary>
    public sealed class LotteryCard
    {
        private CardLotteryMaster _cardLotteryMaster;

        private ProbabilityCalculator _calculator;


        public LotteryCard()
        {
            _calculator = new ProbabilityCalculator(Random.Range(0, Int32.MaxValue));
            _cardLotteryMaster = Resources.Load<CardLotteryMaster>("MasterData/CardLotteryMaster");
        }

        /// <summary>
        /// 抽選する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CommandCardEntity> Lottery(int id)
        {
            var datas = _cardLotteryMaster.FindById(id);
            IEnumerable<CardLotteryData> filter = datas;
            if (datas.Count() > 3)
            {
                filter = datas.Shuffle().Take(3);
            }

            var cardEntities = new List<CommandCardEntity>();
            foreach (var data in filter)
            {
                var entity = new CommandCardEntity(
                    0,
                    (CommandEffectType)data.EffectTypeId,
                    data.Max == 0 ? 0 : Random.Range(data.Min, data.Max + 1)
                );

                cardEntities.Add(entity);
            }

            return cardEntities;
        }
    }
}
