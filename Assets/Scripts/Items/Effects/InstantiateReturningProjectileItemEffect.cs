using System;
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
    public class InstantiateReturningProjectileItemEffect: IItemEffect
    {
        [SerializeField]
        private Projectile _projectilePrefab;
        
        [SerializeField]
        private bool _waitForInput = true;
        
        [SerializeField]
        private float _force = 25;
        
        [SerializeField]
        private float _damageMultiplier = 1f;
        
        [SerializeField]
        private float _returnDamageMultiplier = 1f;
        
        [SerializeField]
        private float _velocityReturnTolerance = .3f;
        
        [SerializeField]
        private float _returnDistanceTolerance = .2f;
        
        [SerializeField]
        private ForceMode2D _forceMode = ForceMode2D.Impulse;
        
        [SerializeField]
        private Color _projectileColor = Color.white;

        [Space(5)] 
        [OdinSerialize] 
        private List<IProjectileEffect> _projectileEffects;
        
        private IPlayerStats _playerStats;
        
        private Projectile _projectile;
        
        private IPlayerStats PlayerStats => _playerStats ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerStats>();
        public async UniTask OnUse(IItemUser user, UsableItem item)
        {
            _projectile = Object
                .Instantiate(_projectilePrefab, user.GameObject.transform.position, Quaternion.identity);

            if (_projectileEffects != null)
                foreach (var effect in _projectileEffects)
                    _projectile.AddEffect(effect);
            
            _projectile.AttackerGroup = EntityAttackerGroup.Player;
            
            _projectile.GetComponent<SpriteRenderer>().color = _projectileColor;
            _projectile.Damage = PlayerStats.GetStatsData().Damage * _damageMultiplier;
            
            var mousePosition = GetMousePosition();
            var direction = (mousePosition - (Vector2) user.GameObject.transform.position).normalized;
            
            _projectile.transform.rotation = 
                Quaternion.Euler(0, 0, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg);

            var projectileRigidbody = _projectile.GetComponent<Rigidbody2D>();
            projectileRigidbody.AddForce(direction * _force, _forceMode);
            
            var projectileDamageSource = _projectile.GetComponent<IDamageSource>();
            projectileDamageSource.Source = user.ParentGameObject;

            await UniTask.WaitUntil(() => projectileRigidbody.velocity.magnitude < _velocityReturnTolerance);
            
            while (true)
            {
                if(!_waitForInput)
                    break;
                
                if(Mouse.current.leftButton.isPressed)
                    break;
                
                await UniTask.Yield();
            }

            _projectile.ResetDamagedEntities();
            _projectile.Damage *= _returnDamageMultiplier;
            _projectile.transform.localScale *= _returnDamageMultiplier;
            
            while (true)
            {
                ProjectileGoToPlayer(projectileRigidbody, user.GameObject.transform);
                await UniTask.WaitForFixedUpdate();
                
                if (Vector2.Distance(_projectile.transform.position, user.GameObject.transform.position) < _returnDistanceTolerance)
                    break;
            }
            
            Object.Destroy(_projectile.gameObject);
        }
        
        private Vector2 GetMousePosition()
        {
            var mousePosition = Camera.main!.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return new Vector2(mousePosition.x, mousePosition.y);
        } 
        
        private void ProjectileGoToPlayer(Rigidbody2D projectileRigidbody, Transform player)
        {
            var direction = (player.position - _projectile.transform.position).normalized;
            projectileRigidbody.AddForce(direction * _force * 5.5f, ForceMode2D.Force);
            
            _projectile.transform.rotation = 
                Quaternion.Euler(0, 0, Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg);
        }
    }
}