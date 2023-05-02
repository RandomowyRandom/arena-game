using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class TargetFollowEntityTask: IEntityTask
    {
        [OdinSerialize]
        private IEntityTargetProvider _targetProvider;

        [SerializeField]
        private float _duration;

        [SerializeField] 
        private float _speed = 40f;
        
        private float _elapsedTime;
        
        private Rigidbody2D _rigidbody;
        
        public async UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            _rigidbody = entity.GetComponent<Rigidbody2D>();
            _elapsedTime = 0;
            var target = _targetProvider.GetTarget();

            if (target == null)
                return EnemyTaskResult.Break;
            
            while (true)
            {
                if (entity == null)
                    return EnemyTaskResult.Break;
                
                if (_elapsedTime >= _duration && _duration != -1)
                    return EnemyTaskResult.Completed;
                
                var direction = (target.transform.position - entity.transform.position).normalized;
                var movementVector = direction * _speed;
            
                _rigidbody.AddForce(movementVector, ForceMode2D.Force);

                _elapsedTime += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}