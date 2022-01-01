using System.Collections.Generic;
using System.Linq;
using Unity1week202112.Data.Master;
using UnityEngine;

namespace Unity1week202112.Domain.Map
{
    public class MapPointEntity
    {
        public int Id { get; }

        public string AreaTitle { get; }

        public int EnemyDeckGroupId { get; }
        
        public int EnemyCharacterId { get; }
        
        public int EnemyHp { get; }

        public MapPointEntity(int id, string areaTitle, int enemyDeckGroupId, int enemyCharacterId, int enemyHp)
        {
            EnemyDeckGroupId = enemyDeckGroupId;
            EnemyCharacterId = enemyCharacterId;
            EnemyHp = enemyHp;
            Id = id;
            AreaTitle = areaTitle;
        }
    }

    public class MapDataFactory
    {
        private readonly MapMaster _master;
        private readonly List<MapPointEntity> _dataPool = new List<MapPointEntity>();

        public MapDataFactory()
        {
            _master = Resources.Load<MapMaster>("MasterData/MapMaster");
        }

        public MapPointEntity Get(int mapId)
        {
            var entity = _dataPool.FirstOrDefault(entity => entity.Id == mapId);
            if (entity == null)
            {
                var raw = _master.Data.First(mapData => mapData.MapId == mapId);
                entity = new MapPointEntity(
                    raw.MapId,
                    raw.AreaName,
                    raw.DeckGroupId,
                    raw.EnemyCharaId,
                    raw.EnemyHp);
                _dataPool.Add(entity);
            }

            return entity;
        }
    }
}
