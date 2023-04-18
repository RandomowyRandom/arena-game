using System;
using Cysharp.Threading.Tasks;
using Items.Abstraction;
using Items.ItemDataSystem;
using Sirenix.Serialization;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class WaitForSecondsItemEffect: IItemEffect
    {
        [OdinSerialize]
        private float _seconds;
        public async UniTask OnUse(IItemUser user, UsableItem item)
        {
            Debug.Log($"Waiting for {_seconds} seconds");
            await UniTask.Delay(TimeSpan.FromSeconds(_seconds));
            Debug.Log($"Waited for {_seconds} seconds");
            
            await UniTask.CompletedTask;
        }
    }
}