using System;
using System.Collections.Generic;
using Sirenix.Serialization;

namespace WorldGeneration
{
    [Serializable]
    public class BiomeGroup
    {
        [OdinSerialize]
        private List<BiomeTerrainGenerator> _biomeTerrainGenerators;
        
        public List<BiomeTerrainGenerator> BiomeTerrainGenerators => _biomeTerrainGenerators;
    }
}