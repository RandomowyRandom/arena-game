using System;
using Items.ItemDataSystem;
using Items.RaritySystem;
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
        
        public Item(ItemData itemData, int amount, GearRarity gearRarity)
        {
            ItemData = itemData;
            Amount = amount;
            GearRarity = gearRarity;
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
        
        public GearRarity GearRarity { get; set; }

        public bool IsRarityItem => GearRarity != null;

        public override string ToString()
        {
            return $"{ItemData.DisplayName} x{Amount}";
        }
    }
}