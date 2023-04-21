using System.Collections.Generic;
using Inventory.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace UI.Inventory
{
    public class InstantiatedSlotsInventoryUI: SerializedMonoBehaviour, IInventoryUI
    {
        [OdinSerialize]
        private IInventory _inventoryToRegister;
        
        [SerializeField]
        private List<ItemSlotUI> _slots = new();
        public IInventory Inventory => _inventory;

        private IInventory _inventory;

        private void Start()
        {
            RegisterInventory(_inventoryToRegister);
        }

        private void OnDestroy()
        {
            DeregisterInventory();
        }
        
        public void RegisterInventory(IInventory inventory)
        {
            if (inventory.Items.Length != _slots.Count)
            {
                Debug.LogError("Inventory and UI slots count mismatch");
                return;
            }
            
            _inventory = inventory;

            for (var i = 0; i < _slots.Count; i++)
            {
                var itemSlotUI = _slots[i];
                
                itemSlotUI.UIHandler = this;
                itemSlotUI.SlotIndex = i;
            }

            _inventory.OnInventoryChanged += UpdateUI;
            UpdateUI();
        }

        public void DeregisterInventory()
        {
            _inventory.OnInventoryChanged -= UpdateUI;
            
            _inventory = null;
        }

        public void UpdateUI()
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var item = _inventory.GetItem(i);
                
                slot.SetItem(item);
            }
        }
    }
}