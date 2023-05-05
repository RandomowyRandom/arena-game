using System.Collections.Generic;
using Common.Attributes;
using EntitySystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WaveSystem
{
    [ScriptableFactoryElement]
    public class WaveFactory: SerializedScriptableObject
    {
        [SerializeField]
        private List<Entity> _entities;
        
        [SerializeField] 
        private float _subWaveCountFactor = 3;
        
        [SerializeField]
        private float _subwaveEnemyCountFactor = 10;
        
        [SerializeField]
        private float _enemyDifficultyFactor = 2;
        
        [SerializeField]
        private float _difficultyRaise = 0.15f;
        
        private float _difficulty = 1;
        private float _subWaveDelay = 2f;
        private int _enemyCap = 3;

        protected override void OnAfterDeserialize()
        {
            _difficulty = 1;
            _enemyCap = 3;
        }

        public Wave GetWave()
        {
            var subWaveCount = Mathf.RoundToInt(_difficulty * _subWaveCountFactor);
            var subWavEnemyCount = Mathf.RoundToInt(_difficulty * _subwaveEnemyCountFactor);

            var subWaves = new List<SubWave>();
            
            for (var i = 0; i < subWaveCount; i++)
            {
                var subWave = new SubWave();
                var entities = GetEntitiesBasedOnDifficulty(subWavEnemyCount);
                
                subWave.Entities = entities;
                subWave.SpawnDelay = _subWaveDelay * .5f;
                
                subWaves.Add(subWave);
            }
            
            _difficulty += _difficultyRaise;
            _enemyCap += Mathf.CeilToInt(1 * _difficulty * .5f);
            _subWaveDelay = Mathf.Clamp(_subWaveDelay - .1f, .25f, 2f);
            
            return new Wave(subWaves, _subWaveDelay, _enemyCap);
        }
        
        private List<Entity> GetEntitiesBasedOnDifficulty(int enemyCount)
        {
            var entities = new List<Entity>();
            var strongestEnemyIndex = GetStrongestEnemyForDifficulty();
            
            for (var i = 0; i < enemyCount; i++)
            {
                if(strongestEnemyIndex - i < 0)
                    continue;
                
                var entity = _entities[strongestEnemyIndex - i];
                entities.Add(entity);
            }
            
            return entities;
        }

        private int GetStrongestEnemyForDifficulty()
        {
            const int toIndex = 1;
            var enemyIndex = Mathf.CeilToInt((_difficulty - toIndex) * _enemyDifficultyFactor);
            return enemyIndex;
        }

        [Button]
        private void TestWaves()
        {
            var wave = GetWave();

            Debug.Log(wave.ToString());
        }
        
        [Button]
        private void ResetDifficulty()
        {
            _difficulty = 1;
            _enemyCap = 3;
        }
    }
}