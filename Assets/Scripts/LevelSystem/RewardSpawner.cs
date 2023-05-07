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
        private PlayerLevel _levelEntity;

        private void Awake()
        {
            _levelEntity = GetComponent<PlayerLevel>();
            _levelEntity.OnLevelUp += SpawnReward;
        }

        private void SpawnReward(Level level)
        {
            var randomPosition = transform.position + new Vector3(
                UnityEngine.Random.Range(-2f, 2f),
                UnityEngine.Random.Range(-2f, 2f),
                0f);
            
            var reward = Instantiate(level.LevelRewardPrefab, randomPosition, Quaternion.identity);
            reward.transform.localScale = Vector3.zero;
            reward.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
        }
    }
}