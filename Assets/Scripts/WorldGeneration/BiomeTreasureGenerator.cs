﻿using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using WorldGeneration.Abstraction;
using WorldGeneration.RoomGeneration;

namespace WorldGeneration
{
    public class BiomeTreasureGenerator: SerializedMonoBehaviour, IFirstStageGenerationStep
    {
        [OdinSerialize]
        private IFirstStageGenerationStep _previousStep;

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
            
                var treasure = Instantiate(roomData.TreasurePrefab, worldPosition, Quaternion.identity, transform);
            
            BiomeGenerationHelper.DisableTilePresenceInRadius(tilePresence, treasurePosition, 1);
            
            OnGenerationComplete?.Invoke(roomData, tilePresence);
        }
    }
}