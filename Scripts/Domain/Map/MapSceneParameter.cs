using System;
using Unity1week202112.Domain.Scene;
using UnityEngine;

namespace Unity1week202112.Domain.Map
{
    [Serializable]
    public class MapSceneParameter : ISceneParameter
    {
        public MapPoint StartPoint => _startPoint;

        [SerializeField]
        private MapPoint _startPoint;

        public MapSceneParameter(MapPoint startPoint)
        {
            _startPoint = startPoint;
        }
    }
}
