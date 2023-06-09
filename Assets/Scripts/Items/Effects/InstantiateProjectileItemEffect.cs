﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EntitySystem;
using EntitySystem.Abstraction;
using Items.Abstraction;
using Items.ItemDataSystem;
using Player.Interfaces;
using ProjectileSystem;
using ProjectileSystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Items.Effects
{
    [Serializable]
    public class InstantiateProjectileItemEffect: IItemEffect
    {
        [SerializeField]
        private Projectile _projectilePrefab;
        
        [SerializeField]
        private float _force = 25;
        
        [SerializeField]
        private ForceMode2D _forceMode = ForceMode2D.Impulse;
        
        [SerializeField]
        private Color _projectileColor = Color.white;

        [Space(5)] 
        [OdinSerialize] 
        private List<IProjectileEffect> _projectileEffects;
        
        private IPlayerStats _playerStats;
        private IPlayerStats PlayerStats => _playerStats ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerStats>();
        
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            var projectile = Object
                .Instantiate(_projectilePrefab, user.GameObject.transform.position, Quaternion.identity);

            if (_projectileEffects != null)
                foreach (var effect in _projectileEffects)
                    projectile.AddEffect(effect);
            
            projectile.AttackerGroup = EntityAttackerGroup.Player;
            
            projectile.GetComponent<SpriteRenderer>().color = _projectileColor;
            projectile.Damage = PlayerStats.GetStatsData().Damage;
            
            var mousePosition = GetMousePosition();
            var direction = (mousePosition - (Vector2) user.GameObject.transform.position).normalized;
            
            projectile.transform.rotation = 
                Quaternion.Euler(0, 0, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg);
            

            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _force, _forceMode);
            
            var projectileDamageSource = projectile.GetComponent<IDamageSource>();
            projectileDamageSource.Source = user.ParentGameObject;
            
            return UniTask.CompletedTask;
        }
        
        private Vector2 GetMousePosition()
        {
            var mousePosition = Camera.main!.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return new Vector2(mousePosition.x, mousePosition.y);
        } 
    }
}