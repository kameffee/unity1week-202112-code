using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity1week202112.Data;
using Unity1week202112.Data.Master;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Command.Effect;
using Unity1week202112.Domain.Deck;
using Unity1week202112.Domain.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// プレイヤーのデッキ
    /// </summary>
    public sealed class PlayerDeck
    {
        public IReadOnlyList<CommandCardModel> Cards => _cardList;
        private List<CommandCardModel> _cardList = new List<CommandCardModel>();

        private Queue<CommandCardModel> _drawQueue;

        private readonly IDeckRepository _deckRepository;

        /// <summary>
        /// プレイヤー側から使う想定
        /// </summary>
        /// <param name="deckParamDatas"></param>
        public PlayerDeck(IDeckRepository deckRepository)
        {
            _deckRepository = deckRepository;

            var loadedDeckEntity = _deckRepository.Load();
            foreach (var cardEntity in loadedDeckEntity.CardList)
            {
                var card = new CommandCardModel(
                    cardEntity.Id,
                    "名前",
                    cardEntity.Value,
                    CreateCommandEffect(cardEntity.EffectType, cardEntity.Value));

                _cardList.Add(card);
            }

            // シャッフルして引く順番をセット
            _drawQueue = new Queue<CommandCardModel>(_cardList.Shuffle());
        }

        /// <summary>
        /// 引くことができるか
        /// </summary>
        public bool Drawable() => _drawQueue.Any();

        public CommandCardModel Draw()
        {
            return _drawQueue.Dequeue();
        }

        public void Reset()
        {
            _cardList.ForEach(model => model.Reset());
            _drawQueue = new Queue<CommandCardModel>(_cardList.Shuffle());
        }

        private ICommandEffect CreateCommandEffect(CommandEffectType effectType, int value)
        {
            ICommandEffect commandEffect = effectType switch
            {
                CommandEffectType.Attack => new AttackCommandEffect(value),
                CommandEffectType.Defence => new DefenceCommandEffect(),
                CommandEffectType.DoubleAttack => new DoubleAttackCommandEffect(value),
                CommandEffectType.Heal => new HpCommandEffect(value),
                CommandEffectType.CounterAttack => new CounterAttackCommandEffect(),
                CommandEffectType.BlockSelect => new UseLockDebuffCommandEffect(value),
                _ => throw new ArgumentOutOfRangeException()
            };

            return commandEffect;
        }
    }

    public class CommandEffectFactory
    {
        private readonly EffectTypeData[] _data;
        private readonly List<CommandEffectTypeEntity> _entities = new List<CommandEffectTypeEntity>();

        public CommandEffectFactory()
        {
            _data = Resources.Load<EffectTypeMaster>("MasterData/EffectTypeMaster").Data.ToArray();
            foreach (var effectTypeData in _data)
            {
                var entity = new CommandEffectTypeEntity(
                    effectTypeData.effectId,
                    effectTypeData.effectName,
                    effectTypeData.description);
                _entities.Add(entity);
            }
        }

        public CommandEffectTypeEntity Get(int effectTypeId)
        {
            return _entities.FirstOrDefault(data => data.Id == effectTypeId);
        }
    }
}
