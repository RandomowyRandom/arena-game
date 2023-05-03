using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class ShootProjectileEntityTask: IEntityTask
    {
        [OdinSerialize]
        private IEntityTargetProvider _targetProvider;
        
        [OdinSerialize]
        private Rigidbody2D _projectilePrefab;
        
        [OdinSerialize]
        private bool _predictTargetPosition;
        
        [OdinSerialize]
        private float _force = 40f;
        
        private Rigidbody2D _targetRigidbody;
        
        public UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            var target = _targetProvider.GetTarget();
            _targetRigidbody ??= target.GetComponent<Rigidbody2D>();
            
            if(target == null || entity == null)
                return UniTask.FromResult(EnemyTaskResult.Break);

            var projectile = Object.Instantiate(_projectilePrefab, entity.transform.position, Quaternion.identity);
            if (!_predictTargetPosition)
            {
                var direction = (target.transform.position - entity.transform.position).normalized;
                projectile.AddForce(direction * _force, ForceMode2D.Impulse);
            }
            else
            {
                var distance = Vector2.Distance(entity.transform.position, target.transform.position);
                var time = distance / _force;
                var targetPosition = target.transform.position + (Vector3)_targetRigidbody.velocity * time;
                var newDirection = (targetPosition - entity.transform.position).normalized;
                projectile.AddForce(newDirection * _force, ForceMode2D.Impulse);
            }
            
            return UniTask.FromResult(EnemyTaskResult.Completed);
        }
    }
}