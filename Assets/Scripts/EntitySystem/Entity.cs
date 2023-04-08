using System;
using TriangularAssets;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Entity : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private EntityData _data;

        public event Action<float> OnDamageTaken;
        public event Action OnDeath;
        
        private float _health;

        private void Awake()
        {
            _health = _data.MaxHealth;
        }
        
        public void TakeDamage(float damage)
        {
            _health -= damage;
            OnDamageTaken?.Invoke(_health);

            if (!(_health <= 0)) 
                return;
            
            OnDeath?.Invoke();
            Destroy(gameObject);
        }

        public void OnDamageSourceEnter(GameObject gameObjectCollision)
        {
            var damageSource = gameObjectCollision.GetComponent<IDamageSource>();
            
            // TODO: check if damage source game object is == damageable game object; if so, ignore

            TakeDamage(damageSource.Damage);
        }
    }
}