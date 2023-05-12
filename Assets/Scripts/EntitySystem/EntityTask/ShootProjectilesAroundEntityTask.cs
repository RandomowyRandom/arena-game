using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class ShootProjectilesAroundEntityTask: IEntityTask
    {
        [SerializeField]
        private Rigidbody2D _projectilePrefab;
        
        [SerializeField]
        private float _force;
        
        [SerializeField]
        private float _amount;
        
        public UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            if (entity == null)
                return UniTask.FromResult(EnemyTaskResult.Break);
            
            var angleStep = 360f / _amount;
            var angle = 0f;
            
            for (var i = 0; i < _amount; i++)
            {
                var projectile = Object.Instantiate(_projectilePrefab, entity.transform.position, Quaternion.identity);
                projectile.AddForce(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized * _force, ForceMode2D.Impulse);
                angle += angleStep;
            }
            
            return UniTask.FromResult(EnemyTaskResult.Completed);
        }
    }
}