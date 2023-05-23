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
        
        public void OnObtain(PlayerUpgradeHandler playerUpgradeHandler)
        {
            playerUpgradeHandler.StatsData += _playerStats;
        }

        public void OnRemove(PlayerUpgradeHandler playerUpgradeHandler)
        {
            playerUpgradeHandler.StatsData -= _playerStats;
        }
    }
}