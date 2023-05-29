using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Interfaces;
using Items;
using Items.ItemDataSystem;
using UnityEngine;

namespace Inventory
{
    public class BasicInventory: MonoBehaviour, IInventory
    {
        [SerializeField]
        private int _capacity = 25;
        
        [SerializeField]
        private List<Item> _startingItems = new();
        public event Action OnInventoryChanged;
        
        public Item[] Items => _items;
        
        public IInventory Inventory => this;

        private Item[] _items;

        private void Awake()
        {
            _items = new Item[Capacity];
        }

        private void Start()
        {
            foreach (var item in _startingItems)
            {
                TryAddItem(item);
            }
        }

        public int Capacity => _capacity;
        public bool IsFull => _items.Length >= Capacity;
        
        public Item TryAddItem(Item item)
        {
            var remainingAmount = item.Amount;

            var bestSlotIndex = GetBestSlotForItem(item);
            if (bestSlotIndex != -1)
            {
                var amountToAdd = Mathf.Min(remainingAmount, GetSpaceForItemInSlot(bestSlotIndex, item));
                AddItemAmountToSlot(bestSlotIndex, new Item(item.ItemData, amountToAdd, item.GearRarity, item.AdditionalItemData));
                remainingAmount -= amountToAdd;

                if (remainingAmount == 0)
                {
                    OnInventoryChanged?.Invoke();
                    return null;
                }
            }

            for (var i = 0; i < Capacity; i++)
            {
                if (Items[i] != null) 
                    continue;
                
                var amountToAdd = Mathf.Min(remainingAmount, item.ItemData.MaxStack);
                Items[i] = new Item(item.ItemData, amountToAdd, item.GearRarity, item.AdditionalItemData);
                remainingAmount -= amountToAdd;

                if (remainingAmount != 0) 
                    continue;
                
                OnInventoryChanged?.Invoke();
                    
                return null;
            }

            OnInventoryChanged?.Invoke();
            return new Item(item.ItemData, remainingAmount, item.GearRarity, item.AdditionalItemData);
        }

        public Item TryRemoveItem(Item item)
        {
            var remainingAmount = item.Amount;
            
            var bestSlotIndex = GetBestSlotToRemoveFrom(item.ItemData);

            while (remainingAmount > 0 && bestSlotIndex != -1)
            {
                var amountToRemove = Mathf.Min(remainingAmount, Items[bestSlotIndex].Amount);
                RemoveItemAmountFromSlot(bestSlotIndex, amountToRemove);
                remainingAmount -= amountToRemove;

                if (remainingAmount == 0)
                {
                    OnInventoryChanged?.Invoke();
                    return null;
                }

                bestSlotIndex = GetBestSlotToRemoveFrom(item.ItemData);
            }
            
            OnInventoryChanged?.Invoke();
            return new Item(item.ItemData, remainingAmount, item.GearRarity, item.AdditionalItemData);
        }

        private int GetBestSlotToRemoveFrom(ItemData itemData)
        {
            var bestSlotIndex = -1;
            
            for (var i = 0; i < Capacity; i++)
            {
                var inventoryItem = Items[i];
                if (inventoryItem == null || inventoryItem.ItemData == null || inventoryItem.ItemData != itemData) 
                    continue;
                
                if (bestSlotIndex == -1 || inventoryItem.Amount < Items[bestSlotIndex].Amount)
                    bestSlotIndex = i;
            }
            
            return bestSlotIndex;
        }
        
        public bool HasItem(Item item, out Item howMuchHas)
        {
            var itemData = item.ItemData;
            
            var amount = _items
                .Where(inventoryItem => inventoryItem != null && inventoryItem.ItemData == itemData)
                .Sum(inventoryItem => inventoryItem.Amount);

            howMuchHas = new Item(itemData, amount);
            
            return amount >= item.Amount;
        }

        public bool HasSpaceForItem(Item item)
        {
            var itemData = item.ItemData;
            var itemAmount = item.Amount;
            
            foreach (var inventoryItem in _items)
            {
                if (inventoryItem == null)
                {
                    itemAmount -= itemData.MaxStack;
                }
                else if (inventoryItem.ItemData == itemData)
                {
                    var inventoryItemAmount = inventoryItem.Amount;
                    itemAmount -= itemData.MaxStack - inventoryItemAmount;
                }

                if (itemAmount <= 0)
                    break;
            }
            
            return itemAmount <= 0;
        }

        public void SetItem(int index, Item item)
        {
            if (item is not { Amount: > 0 })
            {
                _items[index] = null;
                OnInventoryChanged?.Invoke();
                return;
            }
            
            _items[index] = item;
            OnInventoryChanged?.Invoke();
        }

        public Item GetItem(int index)
        {
            return _items[index];
        }

        public void Clear()
        {
            _items = new Item[Capacity];
            OnInventoryChanged?.Invoke();
        }

        private int GetBestSlotForItem(Item item)
        {
            var slotsWithSpaceForItem = new List<int>();
            
            for (var i = 0; i < _items.Length; i++)
            {
                var inventoryItem = _items[i];
                var spaceForItem = GetSpaceForItemInSlot(i, item);
                
                if (spaceForItem == 0)
                    continue;
                
                if (inventoryItem == null)
                {
                    slotsWithSpaceForItem.Add(i);
                    continue;
                }

                if (inventoryItem.ItemData == item.ItemData)
                    slotsWithSpaceForItem.Add(i);
            }

            return slotsWithSpaceForItem.Count > 0
                ? slotsWithSpaceForItem
                    .OrderBy(slot => GetSpaceForItemInSlot(slot, item))
                    .FirstOrDefault()
                : -1;
        }

        private void AddItemAmountToSlot(int slot, Item item)
        {
            var amountInSlot = _items[slot] == null ? 0 : _items[slot].Amount;
            SetItem(slot, new Item(item.ItemData, amountInSlot + item.Amount, item.GearRarity, item.AdditionalItemData));
        }

        private int GetSpaceForItemInSlot(int index, Item item)
        {
            var itemData = item.ItemData;
            var inventoryItem = _items[index];
            
            if (inventoryItem == null)
                return itemData.MaxStack;
            
            if (inventoryItem.ItemData != itemData)
                return 0;
            
            return itemData.MaxStack - inventoryItem.Amount;
        }

        private void RemoveItemAmountFromSlot(int slot, int amountToRemove)
        {
            var amountInSlot = _items[slot].Amount;
            var gearRarity = _items[slot].GearRarity;
            var additionalItemData = _items[slot].AdditionalItemData;
            SetItem(slot, new Item(_items[slot].ItemData, amountInSlot - amountToRemove, gearRarity, additionalItemData));
        }
    }
}