using System.Collections.Generic;
using System.Linq;
using Items.Abstraction;
using Items.ItemDataSystem;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Items
{
    public class SpecificItemUseLockHandler: SerializedMonoBehaviour, IItemUseLock
    {
        [OdinSerialize]
        private IUsableItemProvider _usableItemProvider;
        
        private List<SpecificItemUseLock> _specificItemUseLocks = new();
        
        public void LockItem(UsableItem item)
        {
            if(_specificItemUseLocks.Any(useLock => useLock.LockedItem == item))
                return;
            
            _specificItemUseLocks.Add(new SpecificItemUseLock(item));
        }
        
        public void UnlockItem(UsableItem item)
        {
            _specificItemUseLocks.Remove(_specificItemUseLocks.First(useLock => useLock.LockedItem == item));
        }
        
        public bool IsLocked
        {
            get
            {
                return _specificItemUseLocks.
                    Any(useLock => useLock.LockedItem == _usableItemProvider.GetUsableItem());
            }
        }
    }
}