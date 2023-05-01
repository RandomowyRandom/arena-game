using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelSystem
{
    [RequireComponent(typeof(PlayerLevel))]
    public class RewardSpawner: MonoBehaviour
    {
        [FormerlySerializedAs("_rewardPrefab")] [SerializeField]
        private List<GameObject> _rewardPrefabs;
        
        private PlayerLevel _levelEntity;

        private void Awake()
        {
            _levelEntity = GetComponent<PlayerLevel>();
            _levelEntity.OnLevelUp += SpawnReward;
        }

        private void SpawnReward(int obj)
        {
            var randomPosition = new Vector3(
                UnityEngine.Random.Range(-2f, 2f),
                UnityEngine.Random.Range(-2f, 2f),
                0f);
            
            var randomReward = UnityEngine.Random.Range(0, _rewardPrefabs.Count);
            var reward = Instantiate(_rewardPrefabs[randomReward], randomPosition, Quaternion.identity);
            reward.transform.localScale = Vector3.zero;
            reward.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
        }
    }
}