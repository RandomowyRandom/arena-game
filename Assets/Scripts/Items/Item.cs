using System;
using Items.ItemDataSystem;

namespace Items
{
    [Serializable]
    public class Item
    {
        public Item(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;
        }

        public ItemData ItemData { get; protected set; }

        public int Amount { get; protected set; }

        public override string ToString()
        {
            return $"{ItemData.DisplayName} x({Amount})";
        }
    }
}