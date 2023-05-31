using System;
using System.Collections.Generic;
using Common.Extensions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using WorldGeneration.Abstraction;
using WorldGeneration.RoomGeneration;
using Random = UnityEngine.Random;

namespace WorldGeneration
{
    public class BiomeOreGenerator: SerializedMonoBehaviour, IFirstStageGenerationStep
    {
        [SerializeField]
        private Vector2 _randomPositionOffset = new(0.5f, 0.5f);
        
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
            if (!roomData.GenerateResources)
            {
                OnGenerationComplete?.Invoke(roomData, tilePresence);
                return;
            }
            
            var allResourcePoints = GetOrePoints(tilePresence, out var updatedTilePresence, roomData);

            foreach (var data in roomData.ResourceGroups)
            {
                allResourcePoints.Shuffle();
                    
                var groupAmount = Mathf.FloorToInt(allResourcePoints.Count * data.PercentageCoverage);

                for (var i = 0; i < groupAmount; i++)
                {
                    var point = allResourcePoints[i];
                    InstantiateGroup(point, ref updatedTilePresence, data);
                }
                
                allResourcePoints.RemoveRange(0, groupAmount);
            }
        }
        
        private void InstantiateGroup(Vector2 orePoint, ref bool[,] updatedTilePresence, ResourceGroupData data)
        {
            var oreGroup = new ResourceGroup(data);
            oreGroup.GenerateResourcePositions();

            var resourcePositions = oreGroup.GetResourcePositions();

            for (var x = 0; x < oreGroup.Data.GroupSize.x; x++)
            {
                for (var y = 0; y < oreGroup.Data.GroupSize.y; y++)
                {
                    if (!resourcePositions[x, y])
                        continue;

                    var position = new Vector3Int((int)orePoint.x + x, (int)orePoint.y + y);
                    var isTilePresenceWidthOdd = updatedTilePresence.GetLength(0) % 2 == 1;
                    var worldPosition = BiomeGenerationHelper.GetWorldPositionFromOrigin(position, transform, isTilePresenceWidthOdd);

                    if (position.x < 0 || position.x >= updatedTilePresence.GetLength(0) ||
                        position.y < 0 || position.y >= updatedTilePresence.GetLength(1))
                        continue;
                    
                    if (updatedTilePresence[position.x, position.y])
                        continue;

                    var offsetPosition = new Vector3
                        (worldPosition.x + Random.Range(-_randomPositionOffset.x, _randomPositionOffset.x),
                            worldPosition.y + Random.Range(-_randomPositionOffset.y, _randomPositionOffset.y));
                    
                    var ore = Instantiate(oreGroup.Data.GetRandomResourcePrefab(), offsetPosition, Quaternion.identity);
                    updatedTilePresence[position.x, position.y] = true;
                }
            }
        }

        private List<Vector2> GetOrePoints(bool[,] tilePresence, out bool[,] updatedTilePresence, RoomData roomData)
        {
            List<Vector2Int> possibleOrePositions = new();
            
            var tilePresenceWithPossibleOres = (bool[,]) tilePresence.Clone();

            for (var i = 0; i < roomData.RandomPointCount; i++)
            {
                var circleCenter = BiomeGenerationHelper.FindLargestCircleCenter(tilePresenceWithPossibleOres);
                possibleOrePositions.Add(circleCenter);
                
                tilePresenceWithPossibleOres[circleCenter.x, circleCenter.y] = true;
            }
            
            var amountOfPointsToRemove = possibleOrePositions.Count - Mathf.RoundToInt(possibleOrePositions.Count * roomData.PercentageOfAllRandomPoints);
            
            for (var i = 0; i < amountOfPointsToRemove; i++)
            {
                possibleOrePositions.RemoveRandomElement();
            }
            
            var tilePresenceWithOrePoints = (bool[,]) tilePresence.Clone();

            var orePositions = new List<Vector2>();
            
            foreach (var orePosition in possibleOrePositions)
            {
                tilePresenceWithOrePoints[orePosition.x, orePosition.y] = true;

                var position = new Vector2(orePosition.x, orePosition.y);
                orePositions.Add(position);
            }
            
            updatedTilePresence = tilePresenceWithOrePoints;
            
            return orePositions;
        }

    }
}