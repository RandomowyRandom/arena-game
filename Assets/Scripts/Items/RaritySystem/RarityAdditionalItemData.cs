using Items.Abstraction;
using UnityEngine;

namespace Items.RaritySystem
{
    public class RarityAdditionalItemData: IAdditionalItemData
    {
        public GearRarity GearRarity { get; private set; }
        
        public RarityAdditionalItemData(GearRarity gearRarity)
        {
            GearRarity = gearRarity;
        }

        public string GetTooltip()
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(GearRarity.Color)}>{GearRarity.name}\n";
        }
    }
}