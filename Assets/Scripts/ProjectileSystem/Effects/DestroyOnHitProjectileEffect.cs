using System;
using Cysharp.Threading.Tasks;
using EntitySystem;
using ProjectileSystem.Abstraction;

namespace ProjectileSystem.Effects
{
    [Serializable]
    public class DestroyOnHitProjectileEffect: IProjectileEffect
    {
        public UniTask ApplyHitEffect(Projectile projectile, Entity entity)
        {
            UnityEngine.Object.Destroy(projectile.gameObject);
            
            return UniTask.CompletedTask;
        }
    }
}