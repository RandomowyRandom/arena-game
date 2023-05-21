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
        
        private WaveManager _waveManager;
        
        private GameObject _rewardInstance;
        
        private void Awake()
        {
            _waveManager = GetComponent<WaveManager>();
        }

        private void Start()
        {
            _waveManager.OnWaveEnd += SpawnReward;
            _waveManager.OnWaveStart += DestroyReward;
        }

        private void OnDestroy()
        {
            _waveManager.OnWaveEnd -= SpawnReward;
            _waveManager.OnWaveStart -= DestroyReward;
        }

        private void DestroyReward(Wave wave)
        {
            if(_rewardInstance != null)
                Destroy(_rewardInstance);
        }

        private void SpawnReward(Wave wave)
        {
            _rewardInstance = Instantiate(_rewardPrefab, transform.position + (Vector3)_spawnOffset, Quaternion.identity);
        }
    }
}