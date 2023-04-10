using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Items.RaritySystem;
using Sirenix.Serialization;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace Items.ItemDataSystem
{
    [ScriptableFactoryElement]
    public class Weapon: UsableItem, IStatsDataProvider
    {
        [OdinSerialize]
        private List<GearRarityData> _rarityData;
        
        public StatsData GetStatsData(GearRarity rarity)
        {
            return  _rarityData
                .Where(r => r.GearRarity == rarity)
                .Select(r => r.StatsData).FirstOrDefault();
        }
    }
}