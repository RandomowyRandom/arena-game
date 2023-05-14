using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

namespace WorldGeneration
{
    [Serializable]
    public class BiomeGroup
    {
        [OdinSerialize]
        private List<BiomeTerrainGenerator> _biomeTerrainGenerators;
        
        [OdinSerialize]
        private Vector2 _startingBiomePosition;
        
        public List<BiomeTerrainGenerator> BiomeTerrainGenerators => _biomeTerrainGenerators;
        
        public Vector2 StartingBiomePosition => _startingBiomePosition; 
    }
}