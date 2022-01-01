using System;
using UnityEngine;

namespace Unity1week202112.Domain.Scene
{
    [Serializable]
    public class InGameParameter : ISceneParameter
    {
        public int EnemyHp => _enemyHp;

        public int EnemyDeckGroupId => _enemyDeckGroupId;

        public int EnemyCharacterId => _enemyCharacterId;

        public bool IsTutorial => _isTutorial;

        [SerializeField]
        private bool _isTutorial = false;

        [SerializeField]
        private int _enemyDeckGroupId;

        [SerializeField]
        private int _enemyCharacterId;

        [SerializeField]
        private int _enemyHp;

        public InGameParameter(int enemyHp, int enemyDeckGroupId, int enemyCharacterId, bool isTutorial)
        {
            _enemyHp = enemyHp;
            _enemyDeckGroupId = enemyDeckGroupId;
            _enemyCharacterId = enemyCharacterId;
            _isTutorial = isTutorial;
        }

        public override string ToString()
        {
            return $"{nameof(EnemyHp)}: {EnemyHp}, {nameof(EnemyDeckGroupId)}: {EnemyDeckGroupId}, {nameof(EnemyCharacterId)}: {EnemyCharacterId}, {nameof(IsTutorial)}: {IsTutorial}";
        }
    }
}
