using ProjectileSystem;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(Entity))]
    public class EntityProjectileEffectHandler: MonoBehaviour
    {
        private Entity _entity;
        
        private void Awake()
        {
            _entity = GetComponent<Entity>();
        }

        public async void ApplyProjectileEffects(GameObject collision)
        {
            var projectile = collision.GetComponent<Projectile>();

            if(projectile == null || projectile.Effects == null)
                return;
            
            if(projectile.DamagedEntities.Contains(_entity))
                return;
            
            foreach (var effect in projectile.Effects)
            {
                await effect.ApplyHitEffect(projectile, _entity);
            }
        }
    }
}