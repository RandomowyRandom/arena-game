using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class KeepInDistanceEntityTask: IEntityTask
    {
        [OdinSerialize]
        private IEntityTargetProvider _targetProvider;
        
        [OdinSerialize]
        private float _walkTimeout = 2.5f;
        
        [OdinSerialize]
        private float _speed = 40f;
        
        [OdinSerialize]
        private float _backOffSpeedMultiplier = .75f;
        
        [OdinSerialize]
        private float _minDistance = 3f;
        
        [OdinSerialize]
        private float _maxDistance = 5f;
        
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
                
                if (_elapsedTime >= _walkTimeout)
                    return EnemyTaskResult.Completed;
                
                var distance = Vector2.Distance(entity.transform.position, target.transform.position);
                if (distance >= _minDistance && distance <= _maxDistance)
                    return EnemyTaskResult.Completed;
                
                // check if we are too close
                var isTooClose = distance < _minDistance;
                
                var direction = (target.transform.position - entity.transform.position).normalized;
                var movementVector = direction * _speed;
                movementVector *= isTooClose ? -_backOffSpeedMultiplier : 1;
            
                _rigidbody.AddForce(movementVector, ForceMode2D.Force);

                _elapsedTime += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}