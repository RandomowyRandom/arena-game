using System;
using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Items.Abstraction;
using Items.RaritySystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Stats;
using Stats.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.ItemDataSystem
{
    [ScriptableFactoryElement]
    public class Equipment: ItemData, IStatsDataProvider, IGearRaritiesProvider
    {
        [OdinSerialize]
        private List<GearRarityData> _rarityData;
        
        [SerializeField]
        private EquipmentType _equipmentType;
        
        public EquipmentType EquipmentType => _equipmentType;

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
                (commonStat.Damage, 
                    commonStat.Speed,commonStat.FireRate, 
                    commonStat.MaxHealth, 
                    commonStat.Defense + _defenseIncreasePerLevel * i);
                _rarityData.Add(new GearRarityData(newStat));
            }
        }
        
        [SerializeField]
        private float _defenseIncreasePerLevel = 1f;
    }
}