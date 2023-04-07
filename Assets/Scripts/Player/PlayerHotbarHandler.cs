using System;
using Inventory.Interfaces;
using Items.ItemDataSystem;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerHotbarHandler: SerializedMonoBehaviour, IUsableItemProvider
    {
        [OdinSerialize]
        private IInventory _hotbarInventory;
        
        private int _currentHotbarSlot = 0;
        
        private int LastHotbarSlot => _hotbarInventory.Capacity - 1;
        
        public UsableItem GetUsableItem()
        {
            if(_hotbarInventory.Items[_currentHotbarSlot] == null)
                return null;
            
            if(_hotbarInventory.Items[_currentHotbarSlot].ItemData is UsableItem usableItem)
                return usableItem;
            
            return null;
        }

        private void Update()
        {
            var input = Mouse.current.scroll.y.ReadValue();

            _currentHotbarSlot = input switch
            {
                > 0 => _currentHotbarSlot == LastHotbarSlot ? 0 : _currentHotbarSlot + 1,
                < 0 => _currentHotbarSlot == 0 ? LastHotbarSlot : _currentHotbarSlot - 1,
                _ => _currentHotbarSlot
            };
        }
    }
}