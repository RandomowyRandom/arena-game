using System;
using System.Linq;
using Items.Abstraction;
using Items.ItemDataSystem;
using Items.RaritySystem;
using Sirenix.Utilities;
using Stats.Interfaces;
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

        [Space(5)]
        [SerializeField]
        private GearRarity _gearRarity;

        public Item(ItemData itemData, int amount)
        {
            ItemData = itemData;
            Amount = amount;

            if (itemData is IGearRaritiesProvider statsDataProvider)
                GearRarity = statsDataProvider.GetGearRarities().FirstOrDefault()!.GearRarity;
        }
        
        public Item(ItemData itemData, int amount, GearRarity gearRarity)
        {
            ItemData = itemData;
            Amount = amount;
            GearRarity = gearRarity;
        }
        
        // for odin serialization
        public Item()
        {
            Amount = 1;
        }

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
        
        public GearRarity GearRarity
        {
            get => _gearRarity;
            protected set => _gearRarity = value;
        }

        public bool IsRarityItem => GearRarity != null;

        public override string ToString()
        {
            var amount = Amount > 1 ? $"x{Amount.ToString()}" : string.Empty;
            return IsRarityItem ? $"{GearRarity} {ItemData.DisplayName} {amount}" : $"{ItemData.DisplayName} {amount}";
        }

        public string GetTooltip()
        {
            var stringBuilder = new System.Text.StringBuilder();

            stringBuilder.Append(IsRarityItem
                ? $"<color=#{ColorUtility.ToHtmlStringRGB(GearRarity.Color)}>"
                : "<color=\"white\">");

            if (IsRarityItem)
            {
                stringBuilder.Append(GearRarity.ToString());
                stringBuilder.Append(" ");
            }
            
            stringBuilder.AppendLine(ItemData.DisplayName);
            
            if(Amount > 1)
                stringBuilder.AppendLine($"x{Amount.ToString()}");
            
            stringBuilder.Append("<size=80%>");
            stringBuilder.AppendLine("<color=\"white\">");

            if(!ItemData.Description.IsNullOrWhitespace())
                stringBuilder.AppendLine(ItemData.Description);
            
            if (!IsRarityItem)
                return stringBuilder.ToString();

            if (ItemData is not IStatsDataProvider statsItem)
                return stringBuilder.ToString();
            
            var stats = statsItem.GetStatsData(GearRarity);
            
            stringBuilder.AppendLine();
            switch (stats.Damage)
            {
                case > 0:
                    stringBuilder.Append("<color=green>");
                    stringBuilder.AppendLine($"{stats.Damage} Damage");
                    break;
                
                case < 0:
                    stringBuilder.Append("<color=red>");
                    stringBuilder.AppendLine($"{stats.Damage} Damage");
                    break;
            }
            
            switch (stats.Speed)
            {
                case > 0:
                    stringBuilder.Append("<color=green>");
                    stringBuilder.AppendLine($"{stats.Speed} Speed");
                    break;
                
                case < 0:
                    stringBuilder.Append("<color=red>");
                    stringBuilder.AppendLine($"{stats.Speed} Speed");
                    break;
            }

            switch (stats.MaxHealth)
            {
                case > 0:
                    stringBuilder.Append("<color=green>");
                    stringBuilder.AppendLine($"{stats.MaxHealth} Max Health");
                    break;
                
                case < 0:
                    stringBuilder.Append("<color=red>");
                    stringBuilder.AppendLine($"{stats.MaxHealth} Max Health");
                    break;
            }
            
            switch (stats.Defense)
            {
                case > 0:
                    stringBuilder.Append("<color=green>");
                    stringBuilder.AppendLine($"{stats.Defense} Defense");
                    break;
                
                case < 0:
                    stringBuilder.Append("<color=red>");
                    stringBuilder.AppendLine($"{stats.Defense} Defense");
                    break;
            }
            
            return stringBuilder.ToString();
        }
    }
}