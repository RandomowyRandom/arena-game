using System;
using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Items.Abstraction;
using Items.Durability;
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

        [field: SerializeField]
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

        [InfoBox("Requires definition of Common stats!")]
        [Button]
        private void GenerateStats()
        {
            var hasCommonStats = _rarityData
                .Any(r => r.GearRarity.name == "Common");

            var commonStat = _rarityData[0].StatsData;
                
            if (!hasCommonStats)
                throw new NotImplementedException("Common stats are not defined!");

            for (var i = 1; i < 5; i++)
            {
                var newStat = new StatsData
                    (commonStat.Damage + i * _damageIncreasePerLevel, 
                        commonStat.Speed,commonStat.FireRate, 
                        commonStat.MaxHealth, 
                        commonStat.Defense);
                _rarityData.Add(new GearRarityData(newStat));
            }
        }
        
        [SerializeField]
        private float _damageIncreasePerLevel = 1f;
    }
}