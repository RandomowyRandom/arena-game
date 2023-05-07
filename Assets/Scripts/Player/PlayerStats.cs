using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Player.Interfaces;
using QFSW.QC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Stats;
using Stats.Interfaces;
using UnityEngine;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Player
{
    public class PlayerStats: SerializedMonoBehaviour, IPlayerStats
    {
        [SerializeField]
        private StatsData _defaultStats;
        
        [SerializeField]
        private StatsData _cappedStats;
        
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
            
            var finalStats = _statsDataProviders
                .Aggregate(statsData, 
                    (current, statsDataProvider) => current + statsDataProvider
                        .GetStatsData(null));

            return new StatsData(
                
                Mathf.Clamp(finalStats.Damage, 1, _cappedStats.Damage == -1 ? float.MaxValue : _cappedStats.Damage),
                Mathf.Clamp(finalStats.Speed, 1, _cappedStats.Speed == -1 ? float.MaxValue : _cappedStats.Speed),
                Mathf.Clamp(finalStats.FireRate, .1f, _cappedStats.FireRate == -1 ? float.MaxValue : _cappedStats.FireRate),
                Mathf.Clamp(finalStats.MaxHealth, 25, _cappedStats.MaxHealth == -1 ? float.MaxValue : _cappedStats.MaxHealth),
                Mathf.Clamp(finalStats.Defense, 0, _cappedStats.Defense == -1 ? float.MaxValue : _cappedStats.Defense)
            );
        }

        #region QC

        [Command("log-stats", "Logs the player's stats to the console.")] [UsedImplicitly]
        private void CommandLogStats()
        {
            var statsData = GetStatsData();
            Debug.Log($"Damage: {statsData.Damage}");
            Debug.Log($"Speed: {statsData.Speed}");
            Debug.Log($"FireRate: {statsData.FireRate}");
            Debug.Log($"MaxHealth: {statsData.MaxHealth}");
            Debug.Log($"Defense: {statsData.Defense}");
        }
        

        #endregion
    }
}