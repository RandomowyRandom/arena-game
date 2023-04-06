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

        public float Damage => _damage;
        public float Speed => _speed;

        public float FireRate => _fireRate;

        public float MaxHealth => _maxHealth;
        
        public float Defense => _defense;
    }
}