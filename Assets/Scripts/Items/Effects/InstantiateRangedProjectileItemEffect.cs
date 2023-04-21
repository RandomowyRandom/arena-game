using System;
using Cysharp.Threading.Tasks;
using EntitySystem;
using EntitySystem.Abstraction;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Items.Effects
{
    [Serializable]
    public class InstantiateRangedProjectileItemEffect: IItemEffect
    {
        [SerializeField]
        private GameObject _projectilePrefab; // TODO: later change to DamageSource
        
        [SerializeField]
        private float _force = 10;
        
        [SerializeField]
        private ForceMode2D _forceMode = ForceMode2D.Impulse;
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            var projectile = Object
                .Instantiate(_projectilePrefab, user.GameObject.transform.position, Quaternion.identity);

            Debug.Log($"Instantiated projectile at {user.GameObject.transform.position}");
            
            var mousePosition = GetMousePosition();
            var direction = (mousePosition - (Vector2) user.GameObject.transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().AddForce(direction * _force, _forceMode);
            
            var projectileDamageSource = projectile.GetComponent<IDamageSource>();
            projectileDamageSource.Source = user.GameObject;
            
            return UniTask.CompletedTask;
        }
        
        private Vector2 GetMousePosition()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            return new Vector2(mousePosition.x, mousePosition.y);
        }            
    }
}