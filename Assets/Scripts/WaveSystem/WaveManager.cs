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
        [SerializeField]
        private WaveFactory _waveFactory;
        
        [SerializeField]
        private Transform _playerTransform;
        
        public event Action<Wave> OnWaveStart;
        public event Action<Wave> OnWaveEnd;
        public event Action<SubWave> OnSubWaveStart;
        public event Action<SubWave> OnSubWaveEnd;
        
        private Wave _wave;
        
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

        [Button]
        public async void StartWave()
        {
            await SpawnEnemies();
            
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
                    Instantiate(entity, GetRandomPositionOutOfScreen(), Quaternion.identity);
                    
                    await UniTask.Delay(TimeSpan.FromSeconds(subWave.SpawnDelay));
                }
                
                OnSubWaveEnd?.Invoke(subWave);
                await UniTask.Delay(TimeSpan.FromSeconds(_wave.SubWaveDelay));
            }
            
            OnWaveEnd?.Invoke(_wave);
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