using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using EntitySystem.ObjectAwareFollow.Abstraction;
using UnityEngine;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class ObjectAwareTargetFollowEntityTask: IEntityTask
    {
        [SerializeField]
        private float _duration;

        [SerializeField] 
        private float _speed = 40f;
        
        [SerializeField]
        private float _distanceTolerance = 3f;
        
        private float _elapsedTime;
        
        private IObjectAwareFollowManager _followManager;
        
        private IObjectAwareFollowManager FollowManager => 
            _followManager ??= 
                ServiceLocator.ServiceLocator.Instance.Get<IObjectAwareFollowManager>();
        
        public async UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            var rigidbody = entity.GetComponent<Rigidbody2D>();
            _elapsedTime = 0;
            
            while (true)
            {
                if (entity == null)
                    return EnemyTaskResult.Break;
                
                if (_elapsedTime >= _duration && _duration != -1)
                    return EnemyTaskResult.Completed;
                
                var moveVector = FollowManager.GetVectorForTransform(entity.transform, _distanceTolerance);
                rigidbody.AddForce(moveVector * _speed);
                
                _elapsedTime += Time.fixedDeltaTime;
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}