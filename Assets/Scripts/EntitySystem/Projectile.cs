using System;
using DG.Tweening;
using EntitySystem.Abstraction;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Projectile: MonoBehaviour, IDamageSource
    {
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;

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