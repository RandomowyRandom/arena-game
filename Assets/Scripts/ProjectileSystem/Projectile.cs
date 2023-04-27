using System.Collections.Generic;
using DG.Tweening;
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
        public List<IProjectileEffect> Effects => _effects;
        
        private readonly List<IProjectileEffect> _effects = new();
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;

        private readonly List<IDamageable> _damageables = new();
        
        public List<IDamageable> DamagedEntities => _damageables;
        
        public void AddEffect(IProjectileEffect effect)
        {
            _effects.Add(effect);
        }
        
        private void Awake() 
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_rigidbody2D.velocity.magnitude > .3f) 
                return;
            
            _spriteRenderer.DOFade(0f, .2f).OnComplete(() => Destroy(gameObject));
        }

        public GameObject Source { get; set; }
        public float Damage { get; set; }
    }
}