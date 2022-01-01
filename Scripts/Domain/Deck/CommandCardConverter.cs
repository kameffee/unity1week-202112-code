using System;
using System.Collections.Generic;
using Unity1week202112.Domain.Command;
using Unity1week202112.Domain.Command.Effect;
using Unity1week202112.Domain.Deck;

namespace Unity1week202112.Domain
{
    public class CommandCardConverter
    {
        public List<CommandCardModel> Create(IEnumerable<CommandCardEntity> cardEntities)
        {
            var cardList = new List<CommandCardModel>();
            foreach (var cardEntity in cardEntities)
            {
                cardList.Add(ToModel(cardEntity));
            }

            return cardList;
        }

        public CommandCardModel ToModel(CommandCardEntity entity)
        {
            var card = new CommandCardModel(
                entity.Id,
                "名前",
                entity.Value,
                CreateCommandEffect(entity.EffectType, entity.Value));
            return card;
        }

        public CommandCardEntity ToEntity(CommandCardModel model)
        {
            return new CommandCardEntity(model.Id, model.CommandEffect.EffectType, model.EffectNum);
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
}
