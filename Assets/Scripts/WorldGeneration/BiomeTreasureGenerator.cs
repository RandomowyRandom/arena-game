using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using WorldGeneration.Abstraction;
using WorldGeneration.RoomGeneration;

namespace WorldGeneration
{
    public class BiomeTreasureGenerator: SerializedMonoBehaviour, IGenerationStep
    {
        [OdinSerialize]
        private IGenerationStep _previousStep;

        public event Action<RoomData, bool[,]> OnGenerationComplete;

        private void Awake()
        {
            _previousStep.OnGenerationComplete += Generate;
        }
        
        private void OnDestroy()
        {
            _previousStep.OnGenerationComplete -= Generate;
        }

        public void Generate(RoomData roomData, bool[,] tilePresence)
        {
            if (!roomData.GenerateTreasure)
            {
                OnGenerationComplete?.Invoke(roomData, tilePresence);
                return;
            }
            
            var treasurePosition = BiomeGenerationHelper.FindLargestCircleCenter(tilePresence);
            
            var isTilePresenceWidthOdd = tilePresence.GetLength(0) % 2 == 1;
            var worldPosition = BiomeGenerationHelper.GetWorldPositionFromOrigin((Vector3Int)treasurePosition, transform, isTilePresenceWidthOdd);
            
            Debug.DrawLine(worldPosition, (Vector3)worldPosition + Vector3.up * 2, Color.red, 999);
            
            var treasure = Instantiate(roomData.TreasurePrefab, worldPosition, Quaternion.identity);
            
            BiomeGenerationHelper.DisableTilePresenceInRadius(tilePresence, treasurePosition, 2);
            
            OnGenerationComplete?.Invoke(roomData, tilePresence);
        }
    }
}