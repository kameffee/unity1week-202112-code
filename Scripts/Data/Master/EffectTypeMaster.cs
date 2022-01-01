using System.Collections.Generic;
using UnityEngine;

namespace Unity1week202112.Data.Master
{
    public class EffectTypeMaster : ScriptableObject
    {
        public IEnumerable<EffectTypeData> Data => _data;

        [SerializeField]
        internal EffectTypeData[] _data;
    }
}
