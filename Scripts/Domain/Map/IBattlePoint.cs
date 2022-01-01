using UnityEngine;

namespace Unity1week202112.Domain.Map
{
    public interface IBattlePoint
    {
        public int MapPointId { get; }

        public Vector2 WorldPosition { get; }
    }
}
