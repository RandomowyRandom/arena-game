using UnityEngine;

namespace Stats
{
    [System.Serializable]
    public class StatsData
    {
        [SerializeField]
        private float _damage;
        
        [SerializeField]
        private float _speed;
        
        [SerializeField]
        private float _fireRate;
        
        [SerializeField]
        private float _maxHealth;
        
        [SerializeField]
        private float _defense;

        public StatsData(float damage, float speed, float fireRate, float maxHealth, float defense)
        {
            _damage = damage;
            _speed = speed;
            _fireRate = fireRate;
            _maxHealth = maxHealth;
            _defense = defense;
        }

        public StatsData(StatsData data)
        {
            _damage = data.Damage;
            _speed = data.Speed;
            _fireRate = data.FireRate;
            _maxHealth = data.MaxHealth;
            _defense = data.Defense;
        }

        public StatsData()
        {
            _damage = 0;
            _speed = 0;
            _fireRate = 0;
            _maxHealth = 0;
            _defense = 0;
        }

        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public float FireRate
        {
            get => _fireRate;
            set => _fireRate = value;
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set => _maxHealth = value;
        }

        public float Defense
        {
            get => _defense;
            set => _defense = value;
        }

        public static StatsData operator +(StatsData a, StatsData b)
        {
            return new StatsData(
                a.Damage + b.Damage,
                a.Speed + b.Speed,
                a.FireRate + b.FireRate,
                a.MaxHealth + b.MaxHealth,
                a.Defense + b.Defense
            );
        }
        
        public static StatsData operator -(StatsData a, StatsData b)
        {
            return new StatsData(
                a.Damage - b.Damage,
                a.Speed - b.Speed,
                a.FireRate - b.FireRate,
                a.MaxHealth - b.MaxHealth,
                a.Defense - b.Defense
            );
        }
        
        public static StatsData operator -(StatsData a)
        {
            return new StatsData(
                -a.Damage,
                -a.Speed,
                -a.FireRate,
                -a.MaxHealth,
                -a.Defense
            );
        }
    }
}