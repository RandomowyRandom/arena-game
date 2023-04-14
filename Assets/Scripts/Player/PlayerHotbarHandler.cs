using System;
using System.Collections.Generic;
using Inventory.Interfaces;
using Items.ItemDataSystem;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerHotbarHandler: SerializedMonoBehaviour, IUsableItemProvider
    {
        [OdinSerialize]
        private IInventory _hotbarInventory;
        
        [OdinSerialize]
        private InventoryUI _hotbarUI;

        public event Action<ItemData> OnHotbarItemChanged;
        
        private int _currentHotbarSlot = 0;
        
        private int LastHotbarSlot => _hotbarInventory.Capacity - 1;
        
        private bool _initialized;
        private readonly List<HotbarSlotUI> _hotbarSlots = new();
        
        public UsableItem GetUsableItem()
        {
            if(_hotbarInventory.Items[_currentHotbarSlot] == null)
                return null;
            
            if(_hotbarInventory.Items[_currentHotbarSlot].ItemData is UsableItem usableItem)
                return usableItem;
            
            return null;
        }

        private void SetCurrentHotbarSlot(int slotIndex)
        {
            if (!_initialized)
                Initialize();

            _currentHotbarSlot = slotIndex;
            _hotbarSlots[_currentHotbarSlot].SetSelected(true);
            
            for (var i = 0; i < _hotbarSlots.Count; i++)
            {
                if (i == _currentHotbarSlot)
                    continue;
                
                _hotbarSlots[i].SetSelected(false);
            }
            
            OnHotbarItemChanged?.Invoke(_hotbarInventory.Items[_currentHotbarSlot]?.ItemData);
        }

        private void Initialize()
        {
            foreach (var slot in _hotbarUI.Slots)
            {
                var hotbarSlot = slot.GetComponent<HotbarSlotUI>();

                if (hotbarSlot == null)
                    continue;

                _hotbarSlots.Add(hotbarSlot);
            }
            
            _initialized = true;
        }

        private void Update()
        {
            var input = Mouse.current.scroll.y.ReadValue();

            switch (input)
            {
                case < 0:
                    SetCurrentHotbarSlot(_currentHotbarSlot == LastHotbarSlot ? 0 : _currentHotbarSlot + 1);
                    break;
                case > 0:
                    SetCurrentHotbarSlot(_currentHotbarSlot == 0 ? LastHotbarSlot : _currentHotbarSlot - 1);
                    break;
            }
        }
    }
}