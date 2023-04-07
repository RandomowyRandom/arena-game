﻿using Common.Attributes;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace Items.ItemDataSystem
{
    [ScriptableFactoryElement]
    public class Weapon: UsableItem, IStatsDataProvider
    {
        [SerializeField]
        private StatsData _statsData;
        
        public StatsData GetStatsData()
        {
            return _statsData;
        }
    }
}