using System;
using System.Collections.Generic;
using ServiceLocator;
using Stats;
using UnityEngine;

namespace BuffSystem.Abstraction
{
    public interface IBuffHandler: IService
    {
        public event Action OnBuffChanged;
        public GameObject GameObject { get; }
        
        public void UpdateStatsData(StatsData statsData);
        
        public void AddBuff(BuffData buff);
        public void RemoveBuff(Buff buffData);
        public List<Buff> GetBuffs();
    }
}