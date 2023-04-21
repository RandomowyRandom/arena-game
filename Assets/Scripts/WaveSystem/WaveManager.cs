using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EntitySystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WaveSystem
{
    public class WaveManager: SerializedMonoBehaviour, IWaveManager
    {
        [InfoBox("Test only")]
        [SerializeField]
        private Entity _testEntity;
        
        [SerializeField]
        private Transform _playerTransform;
        
        private Wave _wave;
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IWaveManager>(this);
            
            var subWaves = new List<SubWave>
            {
                new (new List<Entity> {_testEntity, _testEntity, _testEntity}, 1f),
                new (new List<Entity> {_testEntity, _testEntity, _testEntity}, 1f),
                new (new List<Entity> {_testEntity, _testEntity, _testEntity}, 1f),
                new (new List<Entity> {_testEntity, _testEntity, _testEntity}, 1f),
                new (new List<Entity> {_testEntity, _testEntity, _testEntity}, 1f),
                new (new List<Entity> {_testEntity, _testEntity, _testEntity}, 1f),
            };
            _wave = new Wave(subWaves, 2f);
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IWaveManager>();
        }

        public void SetWave(Wave wave)
        {
            _wave = wave;
        }

        [Button]
        public async void StartWave()
        {
            await SpawnEnemies();
        }

        private async UniTask SpawnEnemies()
        {
            foreach (var subWave in _wave.SubWaves)
            {
                foreach (var entity in subWave.Entities)
                {
                    Instantiate(entity, GetRandomPositionOutOfScreen(), Quaternion.identity);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(subWave.SpawnDelay));
                }
            }
        }

        private Vector2 GetRandomPositionOutOfScreen()
        {
            const float distanceFromPlayer = 10f;
            
            var randomPosition = UnityEngine.Random.insideUnitCircle.normalized * distanceFromPlayer;
            
            randomPosition *= UnityEngine.Random.Range(1f, 1.3f);
            
            return _playerTransform.position + new Vector3(randomPosition.x, randomPosition.y, 0);
        }
    }
}