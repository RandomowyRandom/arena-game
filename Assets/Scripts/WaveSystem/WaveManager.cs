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
        
        public event Action<Wave> OnWaveStart;
        public event Action<Wave> OnWaveEnd;
        public event Action<SubWave> OnSubWaveStart;
        public event Action<SubWave> OnSubWaveEnd;

        public bool IsWaveInProgress { get; private set; }
        public GameObject GameObject => gameObject;

        private Wave _wave;
        
        [OdinSerialize] [ReadOnly]
        private readonly List<Entity> _spawnedEnemies = new();
        
        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IWaveManager>(this);
            SetWave(_waveFactory.GetWave());
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
            const float distanceFromFireplace = 17.5f;
            
            var randomPosition = UnityEngine.Random.insideUnitCircle.normalized * distanceFromFireplace;
            
            var posToReturn = transform.position + new Vector3(randomPosition.x, randomPosition.y, 0);
            Debug.DrawRay(posToReturn, Vector3.up, Color.red, 100f);
            
            return posToReturn;
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