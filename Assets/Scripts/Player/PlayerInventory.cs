using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using Inventory.Interfaces;
using Items;
using Items.RaritySystem;
using JetBrains.Annotations;
using Player.Interfaces;
using QFSW.QC;
using QFSW.QC.Suggestors.Tags;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Player
{
    public class PlayerInventory: SerializedMonoBehaviour, IInventory, IPlayerInventory
    {
        [OdinSerialize]
        private IInventory _inventory;
        
        [OdinSerialize]
        private IInventory _hotbar;
        
        [SerializeField]
        private ItemDatabase _itemDatabase;
        public IInventory Inventory => this;

        public event Action OnInventoryChanged;
        public int Capacity => _inventory.Capacity + _hotbar.Capacity;
        public bool IsFull => _inventory.IsFull && _hotbar.IsFull;
        public Item[] Items => _inventory.Items.Concat(_hotbar.Items).ToArray();

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerInventory>(this);
        }
        
        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerInventory>();
        }

        public Item TryAddItem(Item item)
        {
            var rest = _inventory.TryAddItem(item);
            
            if (rest != null)
                rest = _hotbar.TryAddItem(rest);
            
            OnInventoryChanged?.Invoke();
            return rest;
        }

        public Item TryRemoveItem(Item item)
        {
            var rest = _inventory.TryRemoveItem(item);
            
            if (rest != null)
                rest = _hotbar.TryRemoveItem(rest);
            
            OnInventoryChanged?.Invoke();

            return rest;
        }

        public bool HasItem(Item item, out Item howMuchHas)
        {
            _inventory.HasItem(item, out var howMuchHasInInventory);
            _hotbar.HasItem(item, out var howMuchHasInHotbar);
            
            var totalAmount = howMuchHasInInventory.Amount + howMuchHasInHotbar.Amount;
            
            howMuchHas = new Item(item.ItemData, totalAmount);
            
            return totalAmount >= item.Amount;
        }

        public bool HasSpaceForItem(Item item)
        {
            return _inventory.HasSpaceForItem(item) || _hotbar.HasSpaceForItem(item);
        }

        public void SetItem(int index, Item item)
        {
            if (index < _inventory.Capacity)
                _inventory.SetItem(index, item);
            else
                _hotbar.SetItem(index - _inventory.Capacity, item);
            
            OnInventoryChanged?.Invoke();
        }

        public Item GetItem(int index)
        {
            return index < _inventory.Capacity ? 
                _inventory.GetItem(index) : 
                _hotbar.GetItem(index - _inventory.Capacity);
        }

        public void Clear()
        {
            _inventory.Clear();
            _hotbar.Clear();
            
            OnInventoryChanged?.Invoke();
        }

        #region QC

        [Command("log-inventory")] [UsedImplicitly]
        private void CommandLogInventory()
        {
            for (var i = 0; i < Capacity; i++)
            {
                var item = Items[i];
                var itemString = item == null
                    ? $"{nameof(PlayerInventory)}[{i}]: Empty"
                    : $"{nameof(PlayerInventory)}[{i}]: {item}";

                Debug.Log(itemString);
            }
        }
        
        
        [Command("add-item")] [UsedImplicitly]
        private void CommandAddItem(string key, int amount)
        {
            var itemData = _itemDatabase.GetItemData(key);
            if (itemData == null)
            {
                Debug.Log($"No item with key {key} found");
            }
            
            var itemToAdd = new Item(itemData, amount);
            var result = TryAddItem(itemToAdd);
            
            Debug.Log($"{nameof(BasicInventory)}: {(result == null ? "Added" : "Failed to add")} {amount} {itemToAdd}");
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
            
            Debug.Log($"{nameof(BasicInventory)}: {(result == null ? "Removed" : "Failed to remove")} x{amount} {key}");
        }

        #endregion
    }
}