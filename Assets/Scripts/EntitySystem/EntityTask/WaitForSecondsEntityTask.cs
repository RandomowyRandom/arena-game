using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class WaitForSecondsEntityTask: IEntityTask
    {
        [OdinSerialize]
        private float _duration;
        
        public async UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_duration));
            
            return entity == null ? EnemyTaskResult.Break : EnemyTaskResult.Completed;
        }
    }
}