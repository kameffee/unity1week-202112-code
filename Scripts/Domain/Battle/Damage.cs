using UnityEngine;

namespace Unity1week202112.Domain
{
    public readonly struct Damage
    {
        public PlayerStatusModel AttackerStatus => _attackerStatus;
        public int Value => _damage;

        private readonly int _damage;
        private readonly PlayerStatusModel _attackerStatus;

        public Damage(int damage, PlayerStatusModel attackerStatus)
        {
            _damage = damage;
            _attackerStatus = attackerStatus;
        }

        public static Damage operator +(Damage damage1, Damage damage2)
        {
            return new Damage(damage1.Value + damage2.Value, damage1.AttackerStatus);
        }

        public static Damage operator +(Damage damage1, int value)
        {
            return new Damage(damage1.Value + value, damage1.AttackerStatus);
        }
        
        public static Damage operator *(Damage damage1, int ratio)
        {
            return new Damage(damage1.Value * ratio, damage1.AttackerStatus);
        }
        
        public static Damage operator *(Damage damage1, float ratio)
        {
            return new Damage(Mathf.FloorToInt(damage1.Value * ratio), damage1.AttackerStatus);
        }

        public static Damage operator -(Damage damage)
        {
            return new Damage(damage.Value, damage.AttackerStatus);
        }
        
        public static Damage operator -(Damage damage, int value)
        {
            return new Damage(Mathf.Max(0, damage.Value - value), damage.AttackerStatus);
        }
    }
}
