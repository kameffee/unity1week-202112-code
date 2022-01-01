using System;
using UnityEngine;

namespace Unity1week202112.Data
{
    [Serializable]
    public class MapData
    {
        // [PK]
        public int MapId => mapId;

        // Resouces/Characters/Character_{charaId:000}
        public int EnemyCharaId => enemyCharaId;

        // 外部キー: DeckParamData.deckGroupId
        public int DeckGroupId => deckGroupId;

        // 敵HP
        public int EnemyHp => enemyHp;

        // 戦闘前のエリア名表示用
        public string AreaName => areaName;

        [SerializeField]
        private int mapId;

        [SerializeField]
        private int enemyCharaId;

        [SerializeField]
        private int deckGroupId;

        [SerializeField]
        private int enemyHp;

        [SerializeField]
        private string areaName;
    }
}
