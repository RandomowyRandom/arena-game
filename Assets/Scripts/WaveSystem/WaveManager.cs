using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EntitySystem;
using JetBrains.Annotations;
using QFSW.QC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace WaveSystem
{
    public class WaveManager: SerializedMonoBehaviour, IWaveManager
    {
        [SerializeField]
        private WaveFactory _waveFactory;
        
        [SerializeField]
        private Transform _playerTransform;
        
        public event Action<Wave> OnWaveStart;
        public event Action<Wave> OnWaveEnd;
        public event Action<SubWave> OnSubWaveStart;
        public event Action<SubWave> OnSubWaveEnd;

        public bool IsWaveInProgress { get; private set; }

        private Wave _wave;
        
        [OdinSerialize] [ReadOnly]
        private readonly List<Entity> _spawnedEnemies = new();
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IWaveManager>(this);
            
            SetWave(_waveFactory.GetWave());
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IWaveManager>();
        }
        
        public void SetWave(Wave wave)
        {
            _wave = wave;

            Debug.Log(wave.ToString());
        }
        public async void StartWave()
        {
            IsWaveInProgress = true;
            await SpawnEnemies();
            IsWaveInProgress = false;
            
            SetWave(_waveFactory.GetWave());
        }

        private async UniTask SpawnEnemies()
        {
            OnWaveStart?.Invoke(_wave);
            
            foreach (var subWave in _wave.SubWaves)
            {
                OnSubWaveStart?.Invoke(subWave);
                foreach (var entity in subWave.Entities)
                {
                    await UniTask.WaitUntil(() => _spawnedEnemies.Count < _wave.EnemyCap);
                    
                    var spawnedEntity = Instantiate(entity, GetRandomPositionOutOfScreen(), Quaternion.identity);
                    
                    _spawnedEnemies.Add(spawnedEntity);
                    spawnedEntity.OnDeath += _ => _spawnedEnemies.Remove(spawnedEntity);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(subWave.SpawnDelay));
                }
                
                OnSubWaveEnd?.Invoke(subWave);
                await UniTask.Delay(TimeSpan.FromSeconds(_wave.SubWaveDelay));
            }
            
            await UniTask.WaitUntil(() => _spawnedEnemies.Count == 0);
            OnWaveEnd?.Invoke(_wave);
        }

        private Vector2 GetRandomPositionOutOfScreen()
        {
            const float distanceFromPlayer = 10f;
            
            var randomPosition = UnityEngine.Random.insideUnitCircle.normalized * distanceFromPlayer;
            
            randomPosition *= UnityEngine.Random.Range(1f, 1.3f);
            
            return _playerTransform.position + new Vector3(randomPosition.x, randomPosition.y, 0);
        }

        #region QC

        [Command("start-wave")] [UsedImplicitly]
        private void CommandStartWave()
        {
            StartWave();
        }

        #endregion
    }
}