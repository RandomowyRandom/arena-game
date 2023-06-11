using System;
using System.Collections.Generic;
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

        private HashSet<IAdditionalItemData> _additionalItemData = new();

        public Item(ItemData itemData, int amount, HashSet<IAdditionalItemData> additionalItemData = null)
        {
            ItemData = itemData;
            Amount = amount;
            
            if(additionalItemData != null)
                _additionalItemData = additionalItemData;
            
            ItemData.OnItemConstructed(this);
        }

        public Item(Item item)
        {
            ItemData = item.ItemData;
            Amount = item.Amount;
            
            if (item._additionalItemData != null)
                _additionalItemData = item._additionalItemData;
            
            ItemData.OnItemConstructed(this);
        }
        
        // for odin serialization
        public Item()
        {
            Amount = 1;
            _additionalItemData ??= new();
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
        
        public HashSet<IAdditionalItemData> AdditionalItemData => 
            _additionalItemData == null ? new() : new(_additionalItemData);

        public override string ToString()
        {
            var amount = Amount > 1 ? $"x{Amount.ToString()}" : string.Empty;
            
            var rarityData = GetAdditionalData<RarityAdditionalItemData>();
            
            return rarityData != null ? $"{rarityData.GearRarity} {ItemData.DisplayName} {amount}" : $"{ItemData.DisplayName} {amount}";
        }

        public bool HasAdditionalData<T>() where T : IAdditionalItemData
        {
            return _additionalItemData != null && _additionalItemData.Any(x => x is T);
        }
        
        public T GetAdditionalData<T>() where T : IAdditionalItemData
        {
            var hasAdditionalData = HasAdditionalData<T>();
            
            if (!hasAdditionalData)
                return default;
            
            return (T) _additionalItemData.First(x => x is T);
        }
        
        public bool AddAdditionalData(IAdditionalItemData additionalItemData)
        {
            _additionalItemData ??= new HashSet<IAdditionalItemData>();

            if (_additionalItemData.Contains(additionalItemData))
                return false;

            _additionalItemData.Add(additionalItemData);
            return true;
        }
        
        public string GetTooltip()
        {
            var stringBuilder = new System.Text.StringBuilder();

            var rarityData = GetAdditionalData<RarityAdditionalItemData>();
            var isRarityItem = rarityData != null;
            
            stringBuilder.Append(isRarityItem
                ? $"<color=#{ColorUtility.ToHtmlStringRGB(rarityData.GearRarity.Color)}>"
                : "<color=\"white\">");

            if (isRarityItem)
            {
                stringBuilder.Append(rarityData.GearRarity.ToString());
                stringBuilder.Append(" ");
            }

            stringBuilder.AppendLine(ItemData.DisplayName);

            if (Amount > 1)
                stringBuilder.AppendLine($"x{Amount.ToString()}");

            stringBuilder.Append("<size=80%>");
            stringBuilder.AppendLine("<color=\"white\">");

            if (!ItemData.Description.IsNullOrWhitespace())
                stringBuilder.AppendLine(ItemData.Description);

            if (!isRarityItem)
                return stringBuilder.ToString();

            if (ItemData is not IStatsDataProvider statsItem)
                return stringBuilder.ToString();

            var stats = statsItem.GetStatsData(rarityData.GearRarity);

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

            stringBuilder.Append("<color=\"white\">");

            if (_additionalItemData is not { Count: > 0 }) 
                return stringBuilder.ToString();

            foreach (var itemData in _additionalItemData)
            {
                stringBuilder.AppendLine(itemData.GetTooltip());
                stringBuilder.Append("<color=\"white\">");
            }
                

            return stringBuilder.ToString();
        }
    }
}