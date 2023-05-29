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
        
        public GearRarity GearRarity
        {
            get => _gearRarity;
            set => _gearRarity = value;
        }

        public StatsData StatsData
        {
            get => _statsData;
            set => _statsData = value;
        }
    }
}