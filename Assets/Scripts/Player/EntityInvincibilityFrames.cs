using System;
using EntitySystem;
using EntitySystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Entity))]
    public class EntityInvincibilityFrames: MonoBehaviour, IDamageLock
    {
        [SerializeField]
        private float _invincibilitySeconds = .8f;
        
        public bool IsLocked => _invincibilityTimer > 0;
        
        private float _invincibilityTimer;
        
        private Entity _entity;
        
        private void Awake()
        {
            _entity = GetComponent<Entity>();
        }
        private void Start()
        {
            _entity.OnDamageTaken += GrantInvincibility;
        }
        private void Update()
        {
            _invincibilityTimer -= Time.deltaTime;
        }
        private void OnDestroy()
        {
            _entity.OnDamageTaken -= GrantInvincibility;
        }
        
        private void GrantInvincibility(float damage, IDamageSource source)
        {
            _invincibilityTimer = _invincibilitySeconds;
        }

    }
}