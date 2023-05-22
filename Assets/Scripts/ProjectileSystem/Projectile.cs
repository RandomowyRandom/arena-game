using System;
using System.Collections.Generic;
using DG.Tweening;
using EntitySystem;
using EntitySystem.Abstraction;
using ProjectileSystem.Abstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ProjectileSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Projectile: SerializedMonoBehaviour, IDamageSource
    {
        [SerializeField]
        private bool _velocityDestruction = true;
        public event Action<Entity> OnEntityHit; 

        public List<IProjectileEffect> Effects => _effects;
        
        private readonly List<IProjectileEffect> _effects = new();
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;

        private readonly List<IDamageable> _damagedEntities = new();

        public EntityAttackerGroup AttackerGroup { get; set; }
        
        public void AddEffect(IProjectileEffect effect)
        {
            _effects.Add(effect);
        }
        
        public void ResetDamagedEntities()
        {
            _damagedEntities.Clear();
        }
        
        private void Awake() 
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!_velocityDestruction) 
                return;
            
            if (_rigidbody2D.velocity.magnitude > .3f) 
                return;
            
            _spriteRenderer.DOFade(0f, .2f).OnComplete(() => Destroy(gameObject));
        }

        public GameObject Source { get; set; }
        public float Damage { get; set; }
        public bool CanAttackEntity(Entity entity)
        {
            var contains = _damagedEntities.Contains(entity);
            
            return !contains;
        }

        public void EntityAttacked(Entity entity)
        {
            _damagedEntities.Add(entity);
            
            OnEntityHit?.Invoke(entity);
        }
    }
}