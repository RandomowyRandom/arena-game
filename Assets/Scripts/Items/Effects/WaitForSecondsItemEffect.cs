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
            await UniTask.Delay(TimeSpan.FromSeconds(_seconds));
        }
    }
}