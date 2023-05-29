using Items.Abstraction;

namespace Items.Durability
{
    public class DurabilityItemData: IAdditionalItemData
    {
        public DurabilityItemData(int maxDurability)
        {
            MaxDurability = maxDurability;
            CurrentDurability = maxDurability;
        }
        
        public DurabilityItemData(int maxDurability, int currentDurability)
        {
            MaxDurability = maxDurability;
            CurrentDurability = currentDurability;
        }
        
        public int MaxDurability { get; }

        public int CurrentDurability { get; set; }
    }
}
