using System.Collections.Generic;
using UnityEngine;

namespace Unity1week202112.Data.Master
{
    /// <summary>
    /// マップ
    /// </summary>
    public class MapMaster : ScriptableObject
    {
        public IEnumerable<MapData> Data => _data;

        [SerializeField]
        internal MapData[] _data;
    }
}
