using System;
using PlayerUpgradeSystem.Abstraction;
using Stats;
using UnityEngine;

namespace PlayerUpgradeSystem.Effects
{
    [Serializable]
    public class StatsPlayerUpgradeEffect: IPlayerUpgradeEffect
    {
        [SerializeField]
        private StatsData _playerStats = new();
        
        [SerializeField]
        private string _displayName;
        
        public void OnObtain(PlayerUpgradeHandler playerUpgradeHandler)
        {
            playerUpgradeHandler.StatsData += _playerStats;
        }

        public void OnRemove(PlayerUpgradeHandler playerUpgradeHandler)
        {
            playerUpgradeHandler.StatsData -= _playerStats;
        }

        public string GetDescription()
        {
            return _displayName;
        }
    }
}