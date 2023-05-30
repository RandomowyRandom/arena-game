using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration
{
    public struct ResourceGroup
    {
        private readonly float _startSpawnChance;
        private readonly float _chanceFollOff;
        private readonly bool[,] _resourcePositions;

        private ResourceGroupData _data;
        
        public ResourceGroupData Data => _data;
        
        public ResourceGroup(ResourceGroupData data)
        {
            _data = data;
            
            if (data.GroupSize.x <= 0 || data.GroupSize.y <= 0)
                throw new ArgumentException("Width and height must be greater than 0");
            
            if(data.GroupSize.x % 2 == 0 || data.GroupSize.y % 2 == 0)
                throw new ArgumentException("Width and height must be odd numbers");

            _startSpawnChance = data.StartSpawnChance;
            _chanceFollOff = data.ChanceFollOff;
            _resourcePositions = new bool[data.GroupSize.x, data.GroupSize.y];
        }

        public bool[,] GetResourcePositions()
        {
            return _resourcePositions;
        }
        
        public void GenerateResourcePositions()
        {
            var center = new Vector2Int(_resourcePositions.GetLength(0) / 2, _resourcePositions.GetLength(1) / 2);
            var chance = _startSpawnChance;

            _resourcePositions[center.x, center.y] = true;

            SetOresAroundPoint(new Vector2Int(center.x, center.y), chance, out var newPoints);
            
            while (newPoints.Count > 0)
            {
                chance -= _chanceFollOff;

                var newerPoints = new List<Vector2Int>();
                
                foreach (var point in newPoints)
                {
                    SetOresAroundPoint(point, chance, out var newPoints2);
                    newerPoints.AddRange(newPoints2);
                }
                
                newPoints = newerPoints;
            }
        }

        private void SetOresAroundPoint(Vector2Int point, float chance, out List<Vector2Int> newPoints)
        {
            newPoints = new List<Vector2Int>();
            
            for (var x = -1; x < 1; x++)
            {
                for (var y = -1; y < 1; y++)
                {
                    if(x == 0 && y == 0)
                        continue;
                    
                    if (point.x + x < 0 || point.x + x >= _resourcePositions.GetLength(0) || point.y + y < 0 || point.y + y >= _resourcePositions.GetLength(1))
                        continue;
                    
                    if(_resourcePositions[point.x + x, point.y + y])
                        continue;

                    if (!(UnityEngine.Random.value < chance)) 
                        continue;
                    
                    _resourcePositions[point.x + x, point.y + y] = true;
                    newPoints.Add(new Vector2Int(point.x + x, point.y + y));
                }
            }
        }
    }
}