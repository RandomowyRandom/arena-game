using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using UnityEngine;

namespace EntitySystem.EntityTask
{
    [Serializable]
    public class PlayAnimationEntityTask: IEntityTask
    {
        [SerializeField] 
        private Animator _animator;
        
        [SerializeField]
        private string _animationName;
        
        [SerializeField]
        private bool _waitUntilAnimationEnds;
        public async UniTask<EnemyTaskResult> Execute(EntityBehaviour entity)
        {
            _animator.Play(_animationName);

            if(_waitUntilAnimationEnds)
                await UniTask.Delay(TimeSpan.FromSeconds(_animator.GetCurrentAnimatorStateInfo(0).length));

            return EnemyTaskResult.Completed;
        }
    }
}