using System;
using System.Collections.Generic;
using Items.RaritySystem;
using PlayerUpgradeSystem.Abstraction;
using Sirenix.OdinInspector;
using Stats;
using Stats.Interfaces;
using UnityEngine;

namespace PlayerUpgradeSystem
{
    public class PlayerUpgradeHandler: MonoBehaviour, IStatsDataProvider, IPlayerUpgradeHandler
    {
        [SerializeField]
        private PlayerUpgrade _playerUpgrade;
        
        public event Action OnUpgradeChanged;

        [Button("Add Upgrade")]
        private void AddUpgrade()
        {
            AddUpgrade(_playerUpgrade);
        }
        
        private List<PlayerUpgrade> _playerUpgrades = new();

        public StatsData StatsData { get; set; } = new();

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerUpgradeHandler>(this);
        }
        
        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerUpgradeHandler>();
        }
        public void AddUpgrade(PlayerUpgrade playerUpgrade)
        {
            _playerUpgrades.Add(playerUpgrade);
            
            foreach (var effect in playerUpgrade.PlayerUpgradeEffects)
            {
                effect.OnObtain(this);
                OnUpgradeChanged?.Invoke();
            }
            
        }
        
        public void RemoveUpgrade(PlayerUpgrade playerUpgrade)
        {
            _playerUpgrades.Remove(playerUpgrade);

            foreach (var effect in playerUpgrade.PlayerUpgradeEffects)
            {
                effect.OnRemove(this);
            }
            
            OnUpgradeChanged?.Invoke();
        }
        
        public StatsData GetStatsData(GearRarity gearRarity)
        {
            return StatsData;
        }
    }
}