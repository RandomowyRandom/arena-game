using System;
using System.Collections.Generic;
using Inventory.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace UI
{
    public class InventoryUI: SerializedMonoBehaviour
    {
        [SerializeField]
        private RectTransform _slotsPanel;
        
        [SerializeField]
        private ItemSlotUI _slotPrefab;
        
        [OdinSerialize]
        private IInventory _inventoryToRegister;
        
        private readonly List<ItemSlotUI> _slots = new();

        private IInventory _inventory;
        
        public List<ItemSlotUI> Slots => _slots;
        public IInventory Inventory => _inventory;

        private void Start()
        {
            RegisterInventory(_inventoryToRegister);
        }

        private void OnDestroy()
        {
            DeregisterInventory();
        }

        private void RegisterInventory(IInventory inventory)
        {
            _inventory = inventory;
            _inventory.OnInventoryChanged += UpdateUI;
            
            foreach (var slot in _slots)
            {
                Destroy(slot.gameObject);
            }
            
            _slots.Clear();
            
            if (_inventory == null)
                return;

            for (var i = 0; i < _inventory.Capacity; i++)
            {
                var newSlot = Instantiate(_slotPrefab, _slotsPanel);
                newSlot.UIHandler = this;
                newSlot.SlotIndex = i;
                _slots.Add(newSlot);
            }
            
            UpdateUI();
        }
        
        private void DeregisterInventory()
        {
            if (_inventory == null)
                return;
            
            _inventory.OnInventoryChanged -= UpdateUI;
        }

        private void UpdateUI()
        {
            for (var i = 0; i < _inventory.Items.Length; i++)
            {
                var item = _inventory.Items[i];
                _slots[i].SetItem(item);
            }
        }
    }
}