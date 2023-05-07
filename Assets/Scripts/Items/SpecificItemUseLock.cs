using System;
using Items.Abstraction;
using Items.ItemDataSystem;
using Sirenix.Serialization;

namespace Items
{
    [Serializable]
    public class SpecificItemUseLock
    {
        [OdinSerialize]
        private UsableItem _lockedItem;
     
        public SpecificItemUseLock(UsableItem lockedItem)
        {
            _lockedItem = lockedItem;
        }
        
        public UsableItem LockedItem => _lockedItem;
    }
}