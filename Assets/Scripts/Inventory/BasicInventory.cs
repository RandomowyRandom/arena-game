using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Interfaces;
using Items;
using Items.ItemDataSystem;
using Items.RaritySystem;
using JetBrains.Annotations;
using Player.Interfaces;
using QFSW.QC;
using QFSW.QC.Actions;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    public class BasicInventory: MonoBehaviour, IInventory
    {
        [SerializeField]
        private ItemDatabase _itemDatabase;
        
        [SerializeField]
        private int _capacity = 25;
     
        public event Action OnInventoryChanged;
        
        public Item[] Items => _items;
        
        public IInventory Inventory => this;

        private Item[] _items;

        private void Awake()
        {
            _items = new Item[Capacity];
        }

        public int Capacity => _capacity;
        public bool IsFull => _items.Length >= Capacity;
        
        public Item TryAddItem(Item item)
        {
            var remainingAmount = item.Amount;

            if (!HasSpaceForItem(item))
            {
                OnInventoryChanged?.Invoke();
                return item;
            }

            var bestSlotIndex = GetBestSlotForItem(item);
            if (bestSlotIndex != -1)
            {
                var amountToAdd = Mathf.Min(remainingAmount, GetSpaceForItemInSlot(bestSlotIndex, item));
                AddItemAmountToSlot(bestSlotIndex, amountToAdd, item.ItemData);
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
                Items[i] = new Item(item.ItemData, amountToAdd);
                remainingAmount -= amountToAdd;

                if (remainingAmount != 0) 
                    continue;
                
                OnInventoryChanged?.Invoke();
                    
                return null;
            }

            OnInventoryChanged?.Invoke();
            return new Item(item.ItemData, remainingAmount);
        }

        public bool TryRemoveItem(Item item)
        {
            if(!HasItem(item))
                return false;
            
            var leftToRemove = item.Amount;
            
            do
            {
                var slotWithLestAmount = GetSlotWithLeastAmount(item.ItemData);
                
                var amountInSlot = _items[slotWithLestAmount].Amount;
                
                var amountToRemoveFromSlot = Mathf.Min(leftToRemove, amountInSlot);
                
                RemoveItemAmountFromSlot(slotWithLestAmount, amountToRemoveFromSlot);
                
                leftToRemove -= amountToRemoveFromSlot;
            } while (leftToRemove > 0);
            
            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool HasItem(Item item)
        {
            var itemData = item.ItemData;
            var itemAmount = (
                from inventoryItem in _items 
                where inventoryItem != null 
                where inventoryItem.ItemData == itemData 
                select inventoryItem.Amount).Sum();

            return itemAmount >= item.Amount;
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
            if (item.Amount <= 0)
            {
                _items[index] = null;
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

            return slotsWithSpaceForItem
                .OrderBy(slot => GetSpaceForItemInSlot(slot, item))
                .FirstOrDefault();
        }

        private void AddItemAmountToSlot(int slot, int amount, ItemData data)
        {
            var amountInSlot = _items[slot] == null ? 0 : _items[slot].Amount;
            SetItem(slot, new Item(data, amountInSlot + amount));
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
            SetItem(slot, new Item(_items[slot].ItemData, amountInSlot - amountToRemove));
        }

        private int GetSlotWithLeastAmount(ItemData itemData)
        {
            var slotsWithItem = new List<int>();
            
            for (var i = 0; i < _items.Length; i++)
            {
                var slotItem = _items[i];
                
                if (slotItem == null)
                    continue;
                
                if (slotItem.ItemData.Key != itemData.Key)
                    continue;
                
                slotsWithItem.Add(i);
            }

            return slotsWithItem
                .OrderBy(slot => _items[slot].Amount)
                .FirstOrDefault();
        }
        
        
    }
}