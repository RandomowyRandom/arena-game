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

        public StatsData()
        {
            _damage = 0;
            _speed = 0;
            _fireRate = 0;
            _maxHealth = 0;
            _defense = 0;
        }

        public float Damage => _damage;
        public float Speed => _speed;

        public float FireRate => _fireRate;

        public float MaxHealth => _maxHealth;
        
        public float Defense => _defense;

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
    }
}