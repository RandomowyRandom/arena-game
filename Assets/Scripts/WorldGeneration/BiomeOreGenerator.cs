using System.Collections.Generic;
using Common.Extensions;
using UnityEngine;

namespace WorldGeneration
{
    [RequireComponent(typeof(BiomeTerrainGenerator))]
    public class BiomeOreGenerator: MonoBehaviour
    {
        [SerializeField]
        private int _randomPointCount = 25;
        
        [SerializeField] [Range(0f, 1f)]
        private float _percentageOfAllRandomPoints = 0.3f;
        
        [SerializeField]
        private List<ResourceGroupData> _resourceGroups;
        
        [SerializeField]
        private Vector2 _randomPositionOffset = new Vector2(0.5f, 0.5f);
        
        private BiomeTerrainGenerator _terrainGenerator;
        
        private void Awake()
        {
            _terrainGenerator = GetComponent<BiomeTerrainGenerator>();
            _terrainGenerator.OnTerrainGenerated += GenerateOres;
        }

        private void OnDestroy()
        {
            _terrainGenerator.OnTerrainGenerated -= GenerateOres;
        }

        private void GenerateOres(bool[,] tilePresence)
        {
            var allResourcePoints = GetOrePoints(tilePresence, out var updatedTilePresence);

            foreach (var data in _resourceGroups)
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
                    var worldPosition = BiomeGenerationHelper.GetWorldPositionFromTilemapPosition(position, transform);

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

        private List<Vector2> GetOrePoints(bool[,] tilePresence, out bool[,] updatedTilePresence)
        {
            List<Vector2Int> possibleOrePositions = new();
            
            var tilePresenceWithPossibleOres = (bool[,]) tilePresence.Clone();

            for (var i = 0; i < _randomPointCount; i++)
            {
                var circleCenter = BiomeGenerationHelper.FindLargestCircleCenter(tilePresenceWithPossibleOres);
                possibleOrePositions.Add(circleCenter);
                
                tilePresenceWithPossibleOres[circleCenter.x, circleCenter.y] = true;
            }
            
            var amountOfPointsToRemove = possibleOrePositions.Count - Mathf.RoundToInt(possibleOrePositions.Count * _percentageOfAllRandomPoints);
            
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