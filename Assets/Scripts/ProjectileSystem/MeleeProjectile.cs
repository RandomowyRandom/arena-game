using System;
using EntitySystem;
using Player.Interfaces;
using UnityEngine;

namespace ProjectileSystem
{
    [RequireComponent(typeof(Projectile))]
    public class MeleeProjectile: MonoBehaviour
    {
        public static event Action<MeleeProjectile> OnMeleeProjectileHit;
        
        private Projectile _projectile;

        private IPlayerMeleeStreak _playerMeleeStreak;
        
        private IPlayerMeleeStreak PlayerMeleeStreak => 
            _playerMeleeStreak ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerMeleeStreak>();
        
        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _projectile.OnEntityHit += OnEntityHit;
        }

        private void Start()
        {
            _projectile.Damage *= PlayerMeleeStreak.DamageMultiplier;
        }

        private void OnDestroy()
        {
            _projectile.OnEntityHit -= OnEntityHit;
        }

        private void OnEntityHit(Entity entity)
        {
            OnMeleeProjectileHit?.Invoke(this);
        }
    }
}