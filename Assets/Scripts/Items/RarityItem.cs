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

        public override bool Equals(object other)
        {
            return other is RarityItem rarityItem && rarityItem.ItemData.Key == ItemData.Key && rarityItem.GearRarity == GearRarity;
        }

        public override int GetHashCode()
        {
            return (_gearRarity != null ? _gearRarity.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"{_gearRarity.name} {ItemData.DisplayName} x{Amount}";
        }
    }
}