using System;
using System.Collections.Generic;
using System.Linq;
using Inventory.Interfaces;
using Items;
using Items.ItemDataSystem;
using Items.RaritySystem;
using JetBrains.Annotations;
using QFSW.QC;
using QFSW.QC.Actions;
using UnityEditor;
using UnityEngine;

namespace Player
{
    public class PlayerInventory: MonoBehaviour, IInventory
    {
        [SerializeField]
        private ItemDatabase _itemDatabase;
        
        [SerializeField]
        private int _capacity = 25;
        
        public event Action OnInventoryChanged;
        
        public Item[] Items => _items;
        
        private Item[] _items;

        private void Awake()
        {
            _items = new Item[Capacity];
        }

        public int Capacity => _capacity;
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
        
        [Command("add-rarity-item")] [UsedImplicitly]
        private IEnumerator<ICommandAction> CommandAddRarityItem(string key, string rarity ,int amount)
        {
            PlayerInventory target = default;
            var targets = InvocationTargetFactory.FindTargets<PlayerInventory>(MonoTargetType.All);

            // load gear rarity from asset
            var gearRarity = AssetDatabase.LoadAssetAtPath<GearRarity>($"Assets/Scriptables/GearRarity/{rarity}.asset");

            yield return new Value("Select inventory:");
            yield return new Choice<PlayerInventory>(targets, t => target = t);
            
            var itemData = _itemDatabase.GetItemData(key);
            if (itemData == null)
            {
                Debug.Log($"No item with key {key} found");
                yield break;
            }
            
            var result = target.TryAddItem(new RarityItem(itemData, amount, gearRarity));
            
            Debug.Log($"{nameof(PlayerInventory)}: {(result ? "Added" : "Failed to add")} {amount} {gearRarity.name} {key}");
        }
        [Command("add-item")] [UsedImplicitly]
        private IEnumerator<ICommandAction> CommandAddItem(string key, int amount)
        {
            PlayerInventory target = default;
            var targets = InvocationTargetFactory.FindTargets<PlayerInventory>(MonoTargetType.All);

            yield return new Value("Select inventory:");
            yield return new Choice<PlayerInventory>(targets, t => target = t);
            
            var itemData = _itemDatabase.GetItemData(key);
            if (itemData == null)
            {
                Debug.Log($"No item with key {key} found");
                yield break;
            }
            
            var result = target.TryAddItem(new Item(itemData, amount));
            
            Debug.Log($"{nameof(PlayerInventory)}: {(result ? "Added" : "Failed to add")} {amount} {key}");
        }
        
        [Command("remove-item")] [UsedImplicitly]
        private void CommandRemoveItem(string key, int amount)
        {
            var itemData = _itemDatabase.GetItemData(key);
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