﻿using System.Collections.Generic;
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

        protected override void OnAfterDeserialize()
        {
            _difficulty = 1;
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
                subWave.SpawnDelay = 1f;
                
                subWaves.Add(subWave);
            }
            
            _difficulty += _difficultyRaise;
            
            return new Wave(subWaves, 2f);
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
        }
    }
}