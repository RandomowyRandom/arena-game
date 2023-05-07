using Cysharp.Threading.Tasks;
using EntitySystem;

namespace ProjectileSystem.Abstraction
{
    public interface IProjectileEffect
    {
        public UniTask ApplyHitEffect(Projectile projectile, Entity entity);
    }
}