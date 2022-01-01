using System.Collections.Generic;
using System.Linq;
using Unity1week202112.Data.Master;
using Unity1week202112.Domain.Command;
using UnityEngine;

namespace Unity1week202112.Domain.Deck
{
    public class PlayerDeckRepository : IDeckRepository
    {
        private CommandDeckEntity _entity;

        public PlayerDeckRepository()
        {
            // 初期
            var loadedDeckMaster = Resources.Load<PlayerDeckMaster>("MasterData/PlayerDeckMaster");

            List<CommandCardEntity> list = new List<CommandCardEntity>();
            int id = 0;
            foreach (var data in loadedDeckMaster.FindById(0))
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
