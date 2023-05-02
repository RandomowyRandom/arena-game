using System.Collections.Generic;
using Stats;
using UnityEngine;

namespace BuffSystem.Abstraction
{
    public interface IBuffHandler
    {
        public GameObject GameObject { get; }
        
        public void UpdateStatsData(StatsData statsData);
        
        public void AddBuff(BuffData buff);
        public void RemoveBuff(Buff buffData);
        public List<Buff> GetBuffs();
    }
}