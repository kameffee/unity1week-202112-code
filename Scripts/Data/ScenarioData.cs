using System;

namespace Unity1week202112.Data
{
    [Serializable]
    public class ScenarioData
    {
        public int ScenarioKey => scenarioKey;
        public int No => no;
        public string Text => text;

        public int scenarioKey;
        public int no;
        public string text;
    }
}
