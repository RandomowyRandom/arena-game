using System;
using Player.Interfaces;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerStats: MonoBehaviour, IPlayerStats, IStatsDataProvider
    {
        [SerializeField]
        private StatsData _defaultStats;
        
        private StatsData _statsData;
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerStats>(this);
        }

        private void Start()
        {
            _statsData = new StatsData(
                _defaultStats.Damage, 
                _defaultStats.Speed, 
                _defaultStats.FireRate, 
                _defaultStats.MaxHealth,
                _defaultStats.Defense
                );
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerStats>();
        }
        
        public void SetStats(StatsData statsData)
        {
            _statsData = statsData;
        }

        public StatsData GetStatsData()
        {
            return _statsData;
        }
    }
}