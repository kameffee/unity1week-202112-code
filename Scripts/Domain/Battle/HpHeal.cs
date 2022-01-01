using UnityEngine;

namespace Unity1week202112.Domain
{
    public readonly struct HpHeal
    {
        /// <summary>
        /// 回復値
        /// </summary>
        public int Heal => _heal;

        /// <summary>
        /// 回復前のHP
        /// </summary>
        public int BeforeHp => _beforeHp;

        /// <summary>
        /// 回復後のHP
        /// </summary>
        public int AfterHp => Mathf.Clamp(_beforeHp + _heal, 0, _max);

        private readonly int _max;
        private readonly int _beforeHp;
        private readonly int _heal;

        public HpHeal(int beforeHp, int max, int heal)
        {
            _beforeHp = beforeHp;
            _max = max;
            _heal = heal;
        }
    }
}