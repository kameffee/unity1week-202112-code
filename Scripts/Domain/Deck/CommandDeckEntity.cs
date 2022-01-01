using System.Collections.Generic;

namespace Unity1week202112.Domain.Deck
{
    public class CommandDeckEntity
    {
        public IReadOnlyList<CommandCardEntity> CardList => _cardList;

        private readonly List<CommandCardEntity> _cardList;

        public CommandDeckEntity(List<CommandCardEntity> cardList)
        {
            _cardList = cardList;
        }

        public void Delete(CommandCardEntity cardEntity)
        {
            _cardList.Remove(cardEntity);
        }

        public void Delete(int id)
        {
            var removeEntity = _cardList.Find(cardEntity => cardEntity.Id == id);
            _cardList.Remove(removeEntity);
        }

        public void Update(int id, CommandCardEntity entity)
        {
            Delete(id);

            // 追加
            _cardList.Add(new CommandCardEntity(id, entity));
        }
    }
}
