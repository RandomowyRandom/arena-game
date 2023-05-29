using Items.Abstraction;
using UnityEngine;

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
        public string GetTooltip()
        {
            return $"Durability: <color=#{ColorUtility.ToHtmlStringRGB(GetDurabilityColor())}> {CurrentDurability}/{MaxDurability}";
        }
        
        public Color GetDurabilityColor()
        {
            var durabilityPercentage = (float)CurrentDurability / MaxDurability;
            return durabilityPercentage switch
            {
                > 0.5f => Color.green,
                > 0.25f => Color.yellow,
                _ => Color.red
            };
        }
    }
}
