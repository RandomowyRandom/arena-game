using System;
using BuffSystem.Abstraction;
using Sirenix.Serialization;
using Stats;

namespace BuffSystem.BuffEffects
{
    [Serializable]
    public class StatsChangeBuffEffect: IBuffEffect
    {
        [OdinSerialize]
        private StatsData _statsData;
        
        public void OnApply(IBuffHandler buffHandler)
        {
            buffHandler.UpdateStatsData(_statsData);
        }

        public void OnRemove(IBuffHandler buffHandler)
        {
            buffHandler.UpdateStatsData(-_statsData);
        }

        public void OnTick(IBuffHandler buffHandler)
        {
        }
    }
}