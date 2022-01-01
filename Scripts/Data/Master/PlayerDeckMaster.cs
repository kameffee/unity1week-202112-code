using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1week202112.Data.Master
{
    public class PlayerDeckMaster : ScriptableObject
    {
        public IEnumerable<PlayerDeckData> Data => _data;
            
        [SerializeField]
        internal PlayerDeckData[] _data;

        public IEnumerable<PlayerDeckData> FindById(int id)
        {
            return Data.Where(data => data.Id == id);
        }
    }
}
