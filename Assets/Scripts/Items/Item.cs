using System;
using Items.ItemDataSystem;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class Item
    {
        [SerializeField]
        private ItemData _itemData;
        
        [SerializeField]
        private int _amount;

        public Item(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
        }
        
        // for odin serialization
        public Item(){}

        public ItemData ItemData
        {
            get => _itemData;
            protected set => _itemData = value;
        }

        public int Amount
        {
            get => _amount;
            protected set => _amount = value;
        }

        public override string ToString()
        {
            return $"{ItemData.DisplayName} x({Amount})";
        }
    }
}