using System;
using Player.Interfaces;
using ProjectileSystem;
using UnityEngine;

namespace Player
{
    public class PlayerMeleeStreak: MonoBehaviour, IPlayerMeleeStreak
    {
        [SerializeField]
        private float _damageIncrease = 0.1f;
        
        [SerializeField]
        private float _maxDamageIncrease = 2f;
        
        public float DamageMultiplier => _currentDamageMultiplier;

        private float _streakTimer;
        
        private float _currentDamageMultiplier = 1f;
        
        private IPlayerStats _playerStats;
        
        private IPlayerStats PlayerStats => 
            _playerStats ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerStats>();

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerMeleeStreak>(this);
        }

        private void Start()
        {
            MeleeProjectile.OnMeleeProjectileHit += OnMeleeProjectileHit;
        }

        private void OnDestroy()
        {
            MeleeProjectile.OnMeleeProjectileHit -= OnMeleeProjectileHit;
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerMeleeStreak>();
        }

        private void OnMeleeProjectileHit(MeleeProjectile projectile)
        {
            _streakTimer = PlayerStats.GetStatsData().FireRate * 2f;
            _currentDamageMultiplier += _damageIncrease;
            
            _currentDamageMultiplier = Mathf.Clamp(_currentDamageMultiplier, 1f, _maxDamageIncrease);
        }

        private void Update()
        {
            _streakTimer -= Time.deltaTime;
            
            if (_streakTimer <= 0f)
                _currentDamageMultiplier = 1f;
        }
    }
}