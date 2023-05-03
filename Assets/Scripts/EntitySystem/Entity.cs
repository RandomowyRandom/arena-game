using System;
using System.Collections.Generic;
using System.Linq;
using EntitySystem.Abstraction;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TriangularAssets;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Entity : SerializedMonoBehaviour, IDamageable
    {
        [SerializeField]
        private EntityData _data;
        
        [OdinSerialize]
        private List<IDamageLock> _damageLocks = new();

        public event Action<float, IDamageSource> OnDamageTaken;
        public event Action<IDamageSource> OnDeath;
        
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

        public void TakeDamage(float damage, IDamageSource source)
        {
            if(_damageLocks != null)
                if (_damageLocks.Any(damageLock => damageLock.IsLocked))
                    return;

            if (PlayerLevel.CurrentLevel < _data.RequiredLevel)
                damage = 0;
            
            _health -= damage;
            OnDamageTaken?.Invoke(damage, source);

            if (!(_health <= 0)) 
                return;
            
            OnDeath?.Invoke(source);
            Destroy(gameObject);
        }

        public void OnDamageSourceEnter(GameObject gameObjectCollision)
        {
            var damageSource = gameObjectCollision.GetComponent<IDamageSource>();
            
            if(damageSource == null)
                return;

            if(gameObject == damageSource.Source)
                return;
            
            if(!damageSource.CanAttackEntity(this))
                return;
            
            if(damageSource.AttackerGroup == _data.AttackerGroup)
                return;
            
            TakeDamage(damageSource.Damage, damageSource);
            
            if(_effectHandler != null)
                _effectHandler.ApplyProjectileEffects(gameObjectCollision);
            
            damageSource.EntityAttacked(this);
        }
    }
}