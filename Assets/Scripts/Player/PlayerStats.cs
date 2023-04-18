using System;
using System.Collections.Generic;
using System.Linq;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerStats: SerializedMonoBehaviour, IPlayerStats
    {
        [SerializeField]
        private StatsData _defaultStats;
        
        [OdinSerialize]
        private List<IStatsDataProvider> _statsDataProviders;
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerStats>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerStats>();
        }

        // TODO: refactor so it is cached and updated when needed
        public StatsData GetStatsData()
        {
            var statsData = new StatsData(
                _defaultStats.Damage, 
                _defaultStats.Speed, 
                _defaultStats.FireRate, 
                _defaultStats.MaxHealth,
                _defaultStats.Defense
            );
            
            return _statsDataProviders
                .Aggregate(statsData, 
                    (current, statsDataProvider) => current + statsDataProvider
                        .GetStatsData(null));
        }
    }
}