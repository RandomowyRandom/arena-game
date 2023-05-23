using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LevelSystem;
using Player.Interfaces;
using PlayerUpgradeSystem;
using PlayerUpgradeSystem.Abstraction;
using UnityEngine;
using WaveSystem;

namespace UI
{
    public class UpgradeSelectionHandlerUI: MonoBehaviour
    {
        [SerializeField]
        private UpgradeUI _upgradeUIPrefab;
        
        [SerializeField]
        private int _maxUpgrades = 3;
        
        [SerializeField]
        private Transform _upgradeUIParent;
        
        [SerializeField]
        private UpgradeDatabase _upgradeDatabase;
        
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        private IPlayerUpgradeHandler _playerUpgradeHandler;
        
        private IPlayerLevel _playerLevel;
        
        private IWaveManager _waveManager;
        
        private List<UpgradeUI> _upgradeUIs = new();

        private int _upgradesToSelect;
        
        private bool _isBusy;

        private void Start()
        {
            _playerUpgradeHandler = ServiceLocator.ServiceLocator.Instance.Get<IPlayerUpgradeHandler>();
            _playerLevel = ServiceLocator.ServiceLocator.Instance.Get<IPlayerLevel>();
            
            _playerLevel.OnLevelUp += HandleLevelUp;
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered += OnServiceRegistered;
        }

        private void OnDestroy()
        {
            _playerLevel.OnLevelUp -= HandleLevelUp;
            ServiceLocator.ServiceLocator.Instance.OnServiceRegistered -= OnServiceRegistered;
        }

        private void HandleLevelUp(Level level)
        {
            _upgradesToSelect++;
        }
        
        private async void TryInstantiateUpgrades(Wave wave)
        {
            if(_upgradesToSelect <= 0)
                return;

            while (true)
            {
                var upgrades = _upgradeDatabase.GetUpgrades();
                upgrades.Shuffle();
                upgrades = upgrades.Take(_maxUpgrades).ToList();

                _isBusy = true;
                
                foreach (var upgrade in upgrades)
                {
                    var upgradeInstance = Instantiate(_upgradeUIPrefab, _upgradeUIParent);
                    upgradeInstance.OnSelected += HandleUpgradeSelected;
                    upgradeInstance.SetUpgrade(upgrade);
                
                    _upgradeUIs.Add(upgradeInstance);
                }
            
                _upgradesToSelect--;
                _canvasGroup.DOFade(1, .2f);

                await UniTask.WaitUntil(() => !_isBusy);
                
                if (_upgradesToSelect <= 0)
                    break;
            }
            
            _canvasGroup.DOFade(0, .2f);
        }
        
        private void HandleUpgradeSelected(UpgradeUI upgradeUI)
        {
            _playerUpgradeHandler.AddUpgrade(upgradeUI.Upgrade);

            foreach (var upgrade in _upgradeUIs)
            {
                upgradeUI.OnSelected -= HandleUpgradeSelected;
                Destroy(upgrade.gameObject);
            }
            
            _upgradeUIs.Clear();

            _upgradeDatabase.UpdateExclusions();
            _upgradeDatabase.ExcludeUpgrade(upgradeUI.Upgrade);

            _isBusy = false;
        }
        
        private void OnServiceRegistered(Type type)
        {
            if (type != typeof(IWaveManager)) 
                return;

            if (_waveManager != null)
            {
                _waveManager.OnWaveEnd -= TryInstantiateUpgrades;
            }
            
            _waveManager = ServiceLocator.ServiceLocator.Instance.Get<IWaveManager>();
            
            _waveManager.OnWaveEnd += TryInstantiateUpgrades;
        }
    }
}