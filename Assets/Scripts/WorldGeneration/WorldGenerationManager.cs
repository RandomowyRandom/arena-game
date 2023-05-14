using System;
using System.Collections.Generic;
using System.Linq;
using Mono.CSharp;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace WorldGeneration
{
    public class WorldGenerationManager: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private List<BiomeGroup> _groups;
        
        private void Start()
        {
            foreach (var group in _groups)
            {
                GenerateGroup(group);
            }
        }

        private void GenerateGroup(BiomeGroup group)
        {
            var mainBiome = group.BiomeTerrainGenerators[0];
            var mainBiomePosition = group.StartingBiomePosition;
            
            mainBiome.BiomePosition = mainBiomePosition;

            var takenDirections = new List<Vector2>();
            for (var i = 1; i < group.BiomeTerrainGenerators.Count; i++)
            {
                var biome = group.BiomeTerrainGenerators[i];

                Vector2 randomDirection;
                while (true)
                {
                    randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                    
                    if(!IsDirectionTooClose(randomDirection, takenDirections))
                        break;
                }
                
                takenDirections.Add(randomDirection);

                var biomePosition = mainBiomePosition 
                    + mainBiome.BiomeSize * .5f
                    - biome.BiomeSize * .5f
                    + mainBiome.BiomeSize * randomDirection;

                biome.BiomePosition = biomePosition;
            }

            foreach (var biome in group.BiomeTerrainGenerators)
            {
                biome.GenerateBiome();
            }
        }
        
        private bool IsDirectionTooClose(Vector2 direction, List<Vector2> listOfTakenDirections)
        {
            return listOfTakenDirections.Any(takenDirection => Vector2.Distance(direction, takenDirection) < .3f);
        }
    }
}