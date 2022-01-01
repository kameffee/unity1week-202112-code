using Unity1week202112.Domain.Map;
using UnityEngine;

namespace Unity1week202112.Presentation.Map
{
    /// <summary>
    /// マップ上のバトルポイント
    /// </summary>
    public class MapFieldBattlePoint : MonoBehaviour, IBattlePoint
    {
        [SerializeField]
        private int _mapPointId;

        public int MapPointId => _mapPointId;

        public Vector2 WorldPosition => transform.position;
    }
}
