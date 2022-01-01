using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unity1week202112.Data.Master
{
    public class CardLotteryMaster : ScriptableObject
    {
        public IEnumerable<CardLotteryData> Data => _data;

        [SerializeField]
        internal CardLotteryData[] _data;

        public IEnumerable<CardLotteryData> FindById(int id)
        {
            return _data.Where(data => data.ID == id);
        }
    }
}
