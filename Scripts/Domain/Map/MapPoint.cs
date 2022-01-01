using System;
using UnityEngine;

namespace Unity1week202112.Domain.Map
{
    [Serializable]
    public struct MapPoint
    {
        public int PointId => _pointId;

        [SerializeField]
        private int _pointId;

        public MapPoint(int pointId) : this()
        {
            _pointId = pointId;
        }

        public static MapPoint operator ++(MapPoint mapPoint)
        {
            return new MapPoint(mapPoint.PointId + 1);
        }
    }
}
