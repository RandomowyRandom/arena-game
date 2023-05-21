using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using WaveSystem;

namespace LevelSystem
{
    [RequireComponent(typeof(PlayerLevel))]
    public class LevelRewardSpawner: MonoBehaviour
    {
        [SerializeField]
        private GameObject _rewardSpawnEffectPrefab;
        
        private PlayerLevel _levelEntity;

        private void Awake()
        {
            _levelEntity = GetComponent<PlayerLevel>();
            _levelEntity.OnLevelUp += SpawnReward;
        }

        private async void SpawnReward(Level level)
        {
            var waveManager = ServiceLocator.ServiceLocator.Instance.Get<IWaveManager>();
            
            await UniTask.WaitUntil(() => !waveManager.IsWaveInProgress);
            
            var position = waveManager.GameObject.transform.position + Vector3.down * 2f;
            
            var reward = Instantiate(level.LevelRewardPrefab, position, Quaternion.identity);
            Instantiate(_rewardSpawnEffectPrefab, position, Quaternion.identity);
            
            reward.transform.localScale = Vector3.zero;
            reward.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
        }
    }
}