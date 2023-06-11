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
        private float _randomOffset;
        
        [SerializeField]
        private AnimationCurve _rewardAmountCurve;
        
        [SerializeField]
        private int _rewardWaveInterval;
        
        private WaveManager _waveManager;
        
        private void Awake()
        {
            _waveManager = GetComponent<WaveManager>();
        }

        private void Start()
        {
            _waveManager.OnWaveEnd += TrySpawnReward;
        }

        private void OnDestroy()
        {
            _waveManager.OnWaveEnd -= TrySpawnReward;
        }

        private void TrySpawnReward(Wave wave)
        {
            if (wave.Index % _rewardWaveInterval != 0) 
                return;
            
            var rewardAmount = Mathf.FloorToInt(_rewardAmountCurve.Evaluate(wave.Index));

            for (var i = 0; i < rewardAmount; i++)
            {
                var spawnPosition = 
                    transform.position + (Vector3)_spawnOffset + new Vector3(UnityEngine.Random.Range(-_randomOffset, _randomOffset), UnityEngine.Random.Range(-_randomOffset, _randomOffset));
                
                Instantiate(_rewardPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}