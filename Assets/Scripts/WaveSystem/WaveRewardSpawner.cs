using System;
using UnityEngine;

namespace WaveSystem
{
    [RequireComponent(typeof(WaveManager))]
    public class WaveRewardSpawner: MonoBehaviour
    {
        [SerializeField]
        private GameObject _rewardPrefab;
        
        [SerializeField]
        private Vector2 _spawnOffset;
        
        [SerializeField]
        private int _rewardWaveInterval;
        
        private WaveManager _waveManager;
        
        private GameObject _rewardInstance;
        
        private void Awake()
        {
            _waveManager = GetComponent<WaveManager>();
        }

        private void Start()
        {
            _waveManager.OnWaveEnd += TrySpawnReward;
            _waveManager.OnWaveStart += TryDestroyReward;
        }

        private void OnDestroy()
        {
            _waveManager.OnWaveEnd -= TrySpawnReward;
            _waveManager.OnWaveStart -= TryDestroyReward;
        }

        private void TryDestroyReward(Wave wave)
        {
            if(_rewardInstance != null)
                Destroy(_rewardInstance);
        }

        private void TrySpawnReward(Wave wave)
        {
            if(wave.Index % _rewardWaveInterval == 0)
                _rewardInstance = Instantiate(_rewardPrefab, transform.position + (Vector3)_spawnOffset, Quaternion.identity);
        }
    }
}