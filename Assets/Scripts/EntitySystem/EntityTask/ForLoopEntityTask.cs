using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class ForLoopEntityTask: IEntityTask
    {
        [OdinSerialize]
        private int _loops;
        
        [OdinSerialize]
        private List<IEntityTask> _tasks;
        
        private UniTask<EnemyTaskResult> _currentTask;
        private EnemyTaskResult _latestTaskResult;
        
        public async UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            for (var i = 0; i < _loops; i++)
            {
                foreach (var task in _tasks)
                {
                    _currentTask = task.Execute(entity);
                    _latestTaskResult = await _currentTask;

                    if (_latestTaskResult == EnemyTaskResult.Break)
                        return EnemyTaskResult.Break;
                }
            }
            
            return EnemyTaskResult.Completed;
        }
    }
}