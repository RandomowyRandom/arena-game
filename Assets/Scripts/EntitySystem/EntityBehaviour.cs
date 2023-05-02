using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(Entity))]
    public class EntityBehaviour: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private List<IEntityTask> _tasks;

        private UniTask<EnemyTaskResult> _currentTask;
        private EnemyTaskResult _latestTaskResult;

        private async void Start()
        {
            while (true)
            {
                foreach (var task in _tasks)
                {
                    _currentTask = task.Execute(this);
                    _latestTaskResult = await _currentTask;

                    if (_latestTaskResult == EnemyTaskResult.Break)
                        break;
                }
                
                if(_latestTaskResult == EnemyTaskResult.Break)
                    break;
            }
        }
    }
}