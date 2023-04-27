using System;
using EntitySystem.Abstraction;
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
        
        public float Health => _health;
        
        public float MaxHealth => _data.MaxHealth;
        
        public bool IsStatic => _data.IsStatic;

        private EntityProjectileEffectHandler _effectHandler;
        
        private float _health;

        private void Awake()
        {
            _health = _data.MaxHealth;
            
            _effectHandler = GetComponent<EntityProjectileEffectHandler>();
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
            
            if(damageSource == null)
                return;
            
            if(gameObject == damageSource.Source)
                return;

            if(damageSource.DamagedEntities.Contains(this))
                return;
            
            TakeDamage(damageSource.Damage);
            
            if(_effectHandler != null)
                _effectHandler.ApplyProjectileEffects(gameObjectCollision);
            
            damageSource.DamagedEntities.Add(this);
        }
    }
}