﻿using System;
using Items;
using Items.ItemDataSystem;

namespace Inventory.Interfaces
{
    public interface IInventory
    {
        public event Action OnInventoryChanged;
        
        public int Capacity { get; }
        public bool IsFull { get; }
        public Item[] Items { get; }
        
        public Item TryAddItem(Item item);
        public Item TryRemoveItem(Item item);
        public bool HasItem(Item item, out Item howMuchHas);
        public bool HasSpaceForItem(Item item);
        public void SetItem(int index, Item item);
        public Item GetItem(int index);
        public void Clear();
    }
}