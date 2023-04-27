using System;
using Cysharp.Threading.Tasks;
using EntitySystem;
using ProjectileSystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectileSystem.Effects
{
    [Serializable]
    public class ApplyKnockBackProjectileEffect: IProjectileEffect
    {
        [SerializeField]
        private float _force = 10f;
        
        public UniTask ApplyHitEffect(Projectile projectile, Entity entity)
        {
            if (entity.IsStatic)
                return UniTask.CompletedTask;
            
            var entityRigidbody = entity.GetComponent<Rigidbody2D>();
            var projectileRigidbody = projectile.GetComponent<Rigidbody2D>();
            
            if(entityRigidbody == null || projectileRigidbody == null)
                return UniTask.CompletedTask;
            
            var direction = projectileRigidbody.velocity.normalized;

            entityRigidbody.AddForce(direction * _force, ForceMode2D.Impulse);
            
            return UniTask.CompletedTask;
        }
    }
}