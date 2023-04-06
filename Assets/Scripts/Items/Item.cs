using System;
using Items.ItemDataSystem;

namespace Items
{
    [Serializable]
    public class Item
    {
        private ItemData _itemData;
        private int _amount;
        
        public Item(ItemData itemData, int amount)
        {
            _itemData = itemData;
            _amount = amount;
        }

        public ItemData ItemData => _itemData;

        public int Amount => _amount;
    }
}