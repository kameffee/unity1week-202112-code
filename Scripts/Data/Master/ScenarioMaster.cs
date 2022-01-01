using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1week202112.Data.Master
{
    public class ScenarioMaster : ScriptableObject
    {
        public IEnumerable<ScenarioData> Data => _data;
     
        [SerializeField]
        internal ScenarioData[] _data;

        public static ScenarioMaster Create()
        {
            var obj = CreateInstance<ScenarioMaster>();
            return obj;
        }

        public IEnumerable<ScenarioData> GetScenerio(int key)
        {
            return _data.Where(data => data.scenarioKey == key);
        }
    }
}
