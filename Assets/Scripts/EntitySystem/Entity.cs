using System;
using EntitySystem.Abstraction;
using Player.Interfaces;
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

        private IPlayerLevel PlayerLevel => _playerLevel ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerLevel>();
        
        private EntityProjectileEffectHandler _effectHandler;
        
        private float _health;
        
        private IPlayerLevel _playerLevel;

        private void Awake()
        {
            _health = _data.MaxHealth;
            
            _effectHandler = GetComponent<EntityProjectileEffectHandler>();
        }

        private void OnDestroy()
        {
            OnDamageTaken = null;
            OnDeath = null;
        }

        public void TakeDamage(float damage)
        {
            if (PlayerLevel.CurrentLevel < _data.RequiredLevel)
                damage = 0;
            
            _health -= damage;
            OnDamageTaken?.Invoke(damage);

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