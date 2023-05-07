using System;
using System.Collections.Generic;
using BuffSystem.Abstraction;
using Items;
using Items.ItemDataSystem;
using Sirenix.Serialization;

namespace BuffSystem.BuffEffects
{
    [Serializable]
    public class LockItemsUsageBuffEffect: IBuffEffect
    {
        [OdinSerialize]
        private List<UsableItem> _lockedItems;
        
        private SpecificItemUseLockHandler _itemLocker;
        
        public void OnApply(IBuffHandler buffHandler)
        {
            _itemLocker ??= buffHandler.GameObject.GetComponent<SpecificItemUseLockHandler>();
            
            foreach (var item in _lockedItems)
            {
                _itemLocker.LockItem(item);
            }
        }

        public void OnRemove(IBuffHandler buffHandler)
        {
            _itemLocker ??= buffHandler.GameObject.GetComponent<SpecificItemUseLockHandler>();

            foreach (var item in _lockedItems)
            {
                _itemLocker.UnlockItem(item);
            }
        }

        public void OnTick(IBuffHandler buffHandler)
        {
            
        }
    }
}