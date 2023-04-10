using Items.ItemDataSystem;
using Items.RaritySystem;

namespace Items
{
    public class RarityItem: Item
    {
        private GearRarity _gearRarity;
        
        public RarityItem(ItemData itemData, int amount, GearRarity rarity) : base(itemData, amount)
        {
            ItemData = itemData;
            Amount = amount;
            _gearRarity = rarity;
        }

        public GearRarity GearRarity => _gearRarity;
        
        public override string ToString()
        {
            return $"{_gearRarity.name} {ItemData.DisplayName} x({Amount})";
        }
    }
}