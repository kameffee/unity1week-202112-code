using System.Collections.Generic;
using Unity1week202112.Data.Master;
using Unity1week202112.Domain.Command;
using UnityEngine;

namespace Unity1week202112.Domain.Deck
{
    /// <summary>
    /// 敵のデッキリポジトリ
    /// </summary>
    public class EnemyDeckRepository : IDeckRepository
    {
        private CommandDeckEntity _entity;

        public EnemyDeckRepository(int deckId)
        {
            // 初期
            var loadedDeckMaster = Resources.Load<PlayerDeckMaster>("MasterData/PlayerDeckMaster");

            List<CommandCardEntity> list = new List<CommandCardEntity>();
            int id = 0;
            foreach (var data in loadedDeckMaster.FindById(deckId))
            {
                for (var i = 0; i < data.Count; i++)
                {
                    list.Add(new CommandCardEntity(id++,
                        (CommandEffectType)data.TypeId,
                        data.Value));
                }
            }

            _entity = new CommandDeckEntity(list);
        }

        public void Save(CommandDeckEntity commandDeckEntity)
        {
            _entity = commandDeckEntity;
        }

        public CommandDeckEntity Load()
        {
            return _entity;
        }
    }
}
