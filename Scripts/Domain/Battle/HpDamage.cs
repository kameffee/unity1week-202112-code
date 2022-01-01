using UnityEngine;

namespace Unity1week202112.Domain
{
    /// <summary>
    /// HPダメージ
    /// </summary>
    public readonly struct HpDamage
    {
        /// <summary>
        /// ダメージ値
        /// </summary>
        public int Damage => _damage;

        /// <summary>
        /// ダメージを受ける前のHP
        /// </summary>
        public int FromHp => _fromHp;

        /// <summary>
        /// ダメージを受ける後のHP
        /// </summary>
        public int ToHp => Mathf.Max(0, _fromHp - _damage);

        private readonly int _fromHp;
        private readonly int _damage;

        public HpDamage(int fromHp, int damage)
        {
            _fromHp = fromHp;
            _damage = damage;
        }
    }
}
