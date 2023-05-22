using System;
using System.Collections.Generic;
using System.Linq;
using EntitySystem.Abstraction;
using Notifications.Abstraction;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TriangularAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EntitySystem
{
    [RequireComponent(typeof(CollisionHandler))]
    public class Entity : SerializedMonoBehaviour, IDamageable
    {
        [SerializeField]
        private EntityData _data;
        
        [OdinSerialize]
        private List<IDamageLock> _damageLocks = new();
        
        [OdinSerialize]
        private List<IDamageProcessor> _damageProcessors = new();

        public event Action<float, IDamageSource> OnDamageTaken;
        public event Action<IDamageSource> OnDeath;

        public event Action<float> OnHeal; 

        public float Health => _health;
        
        public float MaxHealth
        {
            get => _data.MaxHealth;
            set => _data.MaxHealth = value;
        }

        public bool IsStatic => _data.IsStatic;

        private IPlayerLevel PlayerLevel => _playerLevel ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerLevel>();
        
        private IPlayerNotificationHandler NotificationHandler => _notificationHandler ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerNotificationHandler>();
        
        private EntityProjectileEffectHandler _effectHandler;
        
        private float _health;
        
        private IPlayerLevel _playerLevel;
        
        private IPlayerNotificationHandler _notificationHandler;

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

        public void Heal(float amount)
        {
            _health += amount;
            _health = Mathf.Clamp(_health, 0, _data.MaxHealth);
            
            OnHeal?.Invoke(amount);
        }
        
        public void TakeDamage(float damage, IDamageSource source)
        {
            if(gameObject == null)
                return;
            
            if(_damageLocks != null)
                if (_damageLocks.Any(damageLock => damageLock.IsLocked))
                    return;

            if (PlayerLevel.CurrentLevel < _data.RequiredLevel)
            {
                NotificationHandler.TrySendNotification($"Requires level <color=\"red\"> <size=120%> {_data.RequiredLevel}!");
                damage = 0;
            }

            var processedDamage = 
                _damageProcessors.Aggregate(damage, (current, damageProcessor) => 
                    damageProcessor.Process(current));

            _health -= processedDamage;
            OnDamageTaken?.Invoke(processedDamage, source);

            if (!(_health <= 0)) 
                return;
            
            OnDeath?.Invoke(source);
            Destroy(gameObject);
        }
        
        public void InstantKill(IDamageSource source)
        {
            _health = 0;
            OnDamageTaken?.Invoke(99999, source);
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