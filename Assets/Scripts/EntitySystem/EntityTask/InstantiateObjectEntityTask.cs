using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class InstantiateObjectEntityTask: IEntityTask
    {
        [SerializeField]
        private GameObject _prefab;
        
        public UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            Object.Instantiate(_prefab, entity.transform.position, Quaternion.identity);
            
            return UniTask.FromResult(entity == null ? EnemyTaskResult.Break : EnemyTaskResult.Completed);
        }
    }
}