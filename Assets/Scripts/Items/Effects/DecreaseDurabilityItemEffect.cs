using System;
using Cysharp.Threading.Tasks;
using Items.Abstraction;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items.Effects
{
    [Serializable]
    public class DecreaseDurabilityItemEffect: IItemEffect
    {
        [SerializeField]
        private int _durabilityDecreaseAmount;
        
        public UniTask OnUse(IItemUser user, UsableItem item)
        {
            user.DecreaseDurability(item, _durabilityDecreaseAmount);
            return UniTask.CompletedTask;
        }
    }
}