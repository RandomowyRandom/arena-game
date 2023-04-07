using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Interfaces;
using Items;
using Items.ItemDataSystem;
using JetBrains.Annotations;
using QFSW.QC;
using UnityEngine;

namespace Player
{
    public class PlayerInventory: MonoBehaviour, IInventory
    {
        [SerializeField]
        private List<ItemData> _itemDatabase;
        
        public event Action OnInventoryChanged;
        
        private Item[] _items;

        private void Awake()
        {
            _items = new Item[Capacity];
        }

        public int Capacity => 25;
        public bool IsFull => _items.Length >= Capacity;
        
        public bool TryAddItem(Item item)
        {
            if (!HasSpaceForItem(item))
                return false;
            
            var bestSlot = GetBestSlotForItem(item);
            
            if (bestSlot == -1)
                return false;
            
            var leftToAdd = item.Amount;

            do
            {
                bestSlot = GetBestSlotForItem(item);
                var spaceInSlot = GetSpaceForItemInSlot(bestSlot, item);
                
                var amountToFitInSlot = Mathf.Min(leftToAdd, spaceInSlot);

                AddItemAmountToSlot(bestSlot, amountToFitInSlot, item.ItemData);
                
                leftToAdd -= amountToFitInSlot;
            } while (leftToAdd > 0);

            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool TryRemoveItem(Item item)
        {
            // TODO: fix this
            
            if(!HasItem(item))
                return false;
            
            var itemData = item.ItemData;
            var itemAmount = item.Amount;
            
            for (var i = 0; i < _items.Length; i++)
            {
                var inventoryItem = _items[i];
                
                if (inventoryItem.ItemData != itemData)
                    continue;
                
                var inventoryItemAmount = inventoryItem.Amount;
                if (inventoryItemAmount <= itemAmount)
                {
                    _items[i] = null;
                    itemAmount -= inventoryItemAmount;
                }
                else
                {
                    _items[i] = new Item(itemData, inventoryItemAmount - itemAmount);
                    itemAmount = 0;
                }

                if (itemAmount == 0)
                    break;
            }
            
            OnInventoryChanged?.Invoke();
            return true;
        }

        public bool HasItem(Item item)
        {
            var itemData = item.ItemData;
            var itemAmount = _items
                .Where(inventoryItem => inventoryItem.ItemData == itemData)
                .Sum(inventoryItem => inventoryItem.Amount);
            
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

        #region QC

        [Command("log-inventory")] [UsedImplicitly]
        private void CommandLogInventory()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                var item = _items[i];
                
                Debug.Log(item == null
                    ? $"{nameof(PlayerInventory)}[{i}]: Empty"
                    : $"{nameof(PlayerInventory)}[{i}]: {item.ItemData.Key} x{item.Amount}");
            }
        }
        
        [Command("add-item")] [UsedImplicitly]
        private void CommandAddItem(string key, int amount)
        {
            var itemData = _itemDatabase.FirstOrDefault(item => item.Key == key);
            if (itemData == null)
            {
                Debug.Log($"No item with key {key} found");
                return;
            }
            
            var result = TryAddItem(new Item(itemData, amount));
            
            Debug.Log($"{nameof(PlayerInventory)}: {(result ? "Added" : "Failed to add")} {amount} {key}");
        }
        
        [Command("remove-item")] [UsedImplicitly]
        private void CommandRemoveItem(string key, int amount)
        {
            var itemData = _itemDatabase.FirstOrDefault(item => item.Key == key);
            if (itemData == null)
            {
                Debug.Log($"No item with key {key} found");
                return;
            }
            
            var result = TryRemoveItem(new Item(itemData, amount));
            
            Debug.Log($"{nameof(PlayerInventory)}: {(result ? "Removed" : "Failed to remove")} x{amount} {key}");
        }

        #endregion
    }
}