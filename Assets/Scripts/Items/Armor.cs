using Common.Attributes;
using Items.Abstraction;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace Items
{
    [ScriptableFactoryElement]
    public class Armor: ItemData, IStatsDataProvider
    {
        [SerializeField]
        private StatsData _statsData;
        
        public StatsData GetStatsData()
        {
            return _statsData;
        }
    }
}