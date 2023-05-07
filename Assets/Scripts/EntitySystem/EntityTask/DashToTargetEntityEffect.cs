using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class DashToTargetEntityEffect: IEntityTask
    {
        [OdinSerialize]
        private IEntityTargetProvider _targetProvider;
        
        [OdinSerialize]
        private float _force;
        
        private Rigidbody2D _entityRigidbody;
        public UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            _entityRigidbody ??= entity.GetComponent<Rigidbody2D>();
            var target = _targetProvider.GetTarget();
            
            if (target == null)
                return UniTask.FromResult(EnemyTaskResult.Failed);
            
            var direction = (target.transform.position - entity.transform.position).normalized;
            
            _entityRigidbody.AddForce(direction * _force, ForceMode2D.Impulse);
            
            return UniTask.FromResult(EnemyTaskResult.Completed);
        }
    }
}