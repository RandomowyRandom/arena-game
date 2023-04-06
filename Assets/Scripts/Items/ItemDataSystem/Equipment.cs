using Common.Attributes;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace Items.ItemDataSystem
{
    [ScriptableFactoryElement]
    public class Equipment: ItemData, IStatsDataProvider
    {
        [SerializeField]
        private StatsData _statsData;
        
        [SerializeField]
        private EquipmentType _equipmentType;
        
        public StatsData GetStatsData()
        {
            return _statsData;
        }
    }
}