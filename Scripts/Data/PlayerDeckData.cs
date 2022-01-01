using System;
using UnityEngine;

namespace Unity1week202112.Data.Master
{
    [Serializable]
    public class PlayerDeckData
    {
        public int Id => id;
        public int TypeId => typeId;
        public int Value => value;
        public int Count => count;
        
        [SerializeField]
        private int id;
        
        [SerializeField]
        private int typeId;
        
        [SerializeField]
        private int value;
        
        [SerializeField]
        private int count;
    }
}
