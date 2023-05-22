using System;
using EntitySystem;
using Player.Interfaces;
using UnityEngine;

namespace ProjectileSystem
{
    [RequireComponent(typeof(Projectile))]
    public class MeleeProjectile: MonoBehaviour
    {
        [SerializeField]
        private bool _sustainCombo = true;
        public static event Action<MeleeProjectile> OnMeleeProjectileHit;
        
        public bool SustainCombo => _sustainCombo;
        
        private Projectile _projectile;

        private Rigidbody2D _rigidbody2D;
        
        private IPlayerMeleeStreak _playerMeleeStreak;
        
        private IPlayerMeleeStreak PlayerMeleeStreak => 
            _playerMeleeStreak ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerMeleeStreak>();
        
        private void Awake()
        {
            _projectile = GetComponent<Projectile>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            _projectile.OnEntityHit += OnEntityHit;
        }

        private void Start()
        {
            _projectile.Damage *= PlayerMeleeStreak.DamageMultiplier;
            
            var additionalScale = PlayerMeleeStreak.DamageMultiplier - 1f;
            _rigidbody2D.velocity *= PlayerMeleeStreak.DamageMultiplier;
            
            _projectile.transform.localScale += Vector3.one * additionalScale * .5f;
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