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

        public StatsData(float damage, float speed, float fireRate, float maxHealth)
        {
            _damage = damage;
            _speed = speed;
            _fireRate = fireRate;
            _maxHealth = maxHealth;
        }

        public float Damage => _damage;
        public float Speed => _speed;

        public float FireRate => _fireRate;

        public float MaxHealth => _maxHealth;
    }
}