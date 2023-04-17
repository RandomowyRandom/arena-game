using System;
using Inventory.Interfaces;
using Items;
using Items.ItemDataSystem;
using UnityEngine;

namespace Inventory
{
    public class EquipmentInventory: MonoBehaviour, IInventory
    {
        private Item[] _items = new Item[3];
        
        public event Action OnInventoryChanged;
        public int Capacity => _items.Length;
        public bool IsFull => _items.Length >= Capacity;
        public Item[] Items => _items;
        
        private Item HelmetSlot => _items[0];
        private Item ChestplateSlot => _items[1];
        private Item LeggingsSlot => _items[2];
        
        public Item TryAddItem(Item item)
        {
            if(item.ItemData is not Equipment equipmentItem)
                return item;
            
            var slotIndex = GetSlotIndexForEquipment(equipmentItem);
            
            if (_items[slotIndex] != null)
                return item;
            
            SetItem(slotIndex, item);
            
            OnInventoryChanged?.Invoke();
            return null;
        }

        public Item TryRemoveItem(Item item)
        {
            if(item.ItemData is not Equipment equipmentItem)
                return item;
            
            var slotIndex = GetSlotIndexForEquipment(equipmentItem);
            
            if (_items[slotIndex] == null)
                return item;
            
            var removedItem = GetItem(slotIndex);
            SetItem(slotIndex, null);
            
            OnInventoryChanged?.Invoke();
            return removedItem;
        }

        public bool HasItem(Item item, out Item howMuchHas)
        {
            if (item.ItemData is not Equipment equipmentItem)
            {
                howMuchHas = null;
                return false;
            }
            
            var slotIndex = GetSlotIndexForEquipment(equipmentItem);
            
            if (_items[slotIndex] == null)
            {
                howMuchHas = null;
                return false;
            }
            
            howMuchHas = GetItem(slotIndex);
            
            return true;
        }

        public bool HasSpaceForItem(Item item)
        {
            if(item.ItemData is not Equipment equipmentItem)
                return false;
            
            var slotIndex = GetSlotIndexForEquipment(equipmentItem);
            
            return _items[slotIndex] == null;
        }

        public void SetItem(int index, Item item)
        {
            if(item.ItemData is not Equipment equipmentItem)
                return;
            
            var slotIndex = GetSlotIndexForEquipment(equipmentItem);
            
            _items[slotIndex] = item;
            
            OnInventoryChanged?.Invoke();
        }

        public Item GetItem(int index)
        {
            return _items[index];
        }

        public void Clear()
        {
            _items = new Item[3];
            
            OnInventoryChanged?.Invoke();
        }
        
        private int GetSlotIndexForEquipment(Equipment equipmentItem)
        {
            return equipmentItem.EquipmentType switch
            {
                EquipmentType.Helmet => 0,
                EquipmentType.Chestplate => 1,
                EquipmentType.Boots => 2,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}