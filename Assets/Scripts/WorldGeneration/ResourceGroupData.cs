using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using UnityEngine;

namespace WorldGeneration
{
    [Serializable]
    public class ResourceGroupData
    {
        [SerializeField]
        private List<RandomResource> _resourcePrefabs;
        
        [SerializeField] [Range(0f, 1f)]
        private float _startSpawnChance;
        
        [SerializeField] [Range(0f, 1f)]
        private float _chanceFollOff;
        
        [SerializeField]
        private Vector2Int _groupSize;
        
        [SerializeField] [Range(0f, 1f)]
        private float _percentageCoverage = 0.3f;
        
        public GameObject GetRandomResourcePrefab()
        {
            var value = UnityEngine.Random.value;
            
            foreach (var resource in _resourcePrefabs.Where(resource => value <= resource.Chance))
            {
                return resource.ResourcePrefab;
            }
            
            return _resourcePrefabs.GetRandomElement().ResourcePrefab;
        }
        
        public float StartSpawnChance => _startSpawnChance;
        
        public float ChanceFollOff => _chanceFollOff;
        
        public Vector2Int GroupSize => _groupSize;
        
        public float PercentageCoverage => _percentageCoverage;
    }
}