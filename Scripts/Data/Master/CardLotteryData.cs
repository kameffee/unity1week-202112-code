using System;
using UnityEngine;

namespace Unity1week202112.Data.Master
{
    [Serializable]
    public class CardLotteryData
    {
        public int ID => id;

        public int EffectTypeId => effectTypeId;

        public int Min => min;

        public int Max => max;

        public int Ratio => ratio;

        [SerializeField]
        private int id;

        [SerializeField]
        private int effectTypeId;

        [SerializeField]
        private int min;

        [SerializeField]
        private int max;

        [SerializeField]
        private int ratio;
    }
}