using System;
using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Items.Abstraction;
using Items.Durability;
using Items.Effects.Operators;
using Items.Effects.Operators.ConditionEvaluators;
using Items.RaritySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace Items.ItemDataSystem
{
    [ScriptableFactoryElement]
    public class Weapon: UsableItem, IStatsDataProvider, IGearRaritiesProvider
    {
        [SerializeField]
        private int _maxDurability;
        
        [OdinSerialize]
        private List<GearRarityData> _rarityData;
        public int Durability { get; set; }
        
        public void SetRarityData(List<GearRarityData> rarityData)
        {
            _rarityData = new(rarityData);
        }

        public override void OnItemConstructed(Item item)
        {
            var hasItemDurabilityData = item.AdditionalItemData is DurabilityItemData;
            
            if(!hasItemDurabilityData)
                item.AdditionalItemData = new DurabilityItemData(_maxDurability);
        }

        public StatsData GetStatsData(GearRarity rarity)
        {
            return  _rarityData
                .Where(r => r.GearRarity == rarity)
                .Select(r => r.StatsData).FirstOrDefault();
        }
        
        public List<GearRarityData> GetGearRarities()
        {
            return _rarityData;
        }

        [Button("Wrap in Condition")]
        private void WrapInCondition()
        {
            var effects = new List<IItemEffect>
            {
                new ConditionalStatement(new HasEnoughDurabilityEvaluator(), true, Effects)
            };

            SetEffects(effects);
        }
    }
}