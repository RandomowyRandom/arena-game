using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Items.Abstraction;
using Items.RaritySystem;
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
    }
}