using System;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Items.RaritySystem
{
    [Serializable]
    public class GearRarityData
    {
        [SerializeField]
        private GearRarity _gearRarity;
        
        [SerializeField]
        private StatsData _statsData;
        
        public GearRarityData(){}
        
        public GearRarityData(StatsData statsData)
        {
            _statsData = statsData;
        }
        
        public GearRarity GearRarity => _gearRarity;
        
        public StatsData StatsData => _statsData;
    }
}