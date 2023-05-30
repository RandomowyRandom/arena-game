using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration
{
    public static class BiomeGenerationHelper
    {
        public static bool[,] GetTilePresence(Tilemap tilemap1, Tilemap tilemap2)
        {
            var cellBounds = tilemap1.cellBounds;
            var tilePresence = new bool[cellBounds.size.x, cellBounds.size.y];

            for (var x = tilemap1.cellBounds.xMin; x < tilemap1.cellBounds.xMax; x++)
            {
                for (var y = tilemap1.cellBounds.yMin; y < tilemap1.cellBounds.yMax; y++)
                {
                    var tilePos = new Vector3Int(x, y, 0);

                    var tilePresent = (tilemap1.HasTile(tilePos) || tilemap2.HasTile(tilePos));
                    tilePresence[x - tilemap1.cellBounds.xMin, y - cellBounds.yMin] = tilePresent ? true : false;
                }
            }

            return tilePresence;
        }

        public static bool[,] GetTilePresence(Tilemap tilemap)
        {
            var cellBounds = tilemap.cellBounds;
            var tilePresence = new bool[cellBounds.size.x, cellBounds.size.y];
            
            for (var x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (var y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {
                    var tilePos = new Vector3Int(x, y, 0);

                    var tilePresent = tilemap.HasTile(tilePos);
                    tilePresence[x - tilemap.cellBounds.xMin, y - cellBounds.yMin] = tilePresent ? true : false;
                }
            }
            
            return tilePresence;
        }
        
        public static Vector2Int FindLargestCircleCenter(bool[,] grid) {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            float maxRadius = -1;
            var maxX = -1;
            var maxY = -1;
        
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    var radius = FindLargestCircleRadius(x, y, grid);
                    if (!(radius > maxRadius)) 
                        continue;
                    
                    maxRadius = radius;
                    maxX = x;
                    maxY = y;
                }
            }
            return new(maxX, maxY);
        }
        
        public static float FindLargestCircleRadius(int x, int y, bool[,] grid) {
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            var maxRadius = Math.Min(width - x - 1, x);
            maxRadius = Math.Min(maxRadius, Math.Min(height - y - 1, y));
        
            for (var r = maxRadius; r > 0; r--) {
                var overlaps = false;
                
                for (var i = x - r; i <= x + r; i++) {
                    for (var j = y - r; j <= y + r; j++)
                    {
                        if (i < 0 || i >= width || j < 0 || j >= height) 
                            continue;
                        
                        if (!grid[i, j])
                            continue;
                        
                        overlaps = true;
                        break;
                    }
                    if (overlaps) break;
                }
                if (!overlaps) return r;
            }
            return 0;
        }
        
        public static Vector2 GetWorldPositionFromOrigin(Vector3Int tilemapPosition, Transform transform, bool isOdd)
        {
            if (isOdd)
            {
                return new Vector2(tilemapPosition.x + .5f, tilemapPosition.y + .5f)
                       + (Vector2)transform.position;
            }
            
            return new Vector2(tilemapPosition.x + 1, tilemapPosition.y + 1)
                   + (Vector2)transform.position;
        }
        
        public static void DisableTilePresenceInRadius(bool[,] tilePresence, Vector2Int center, int radius)
        {
            for (var x = center.x - radius; x < center.x + radius; x++)
            {
                for (var y = center.y - radius; y < center.y + radius; y++)
                {
                    if (x < 0 || x >= tilePresence.GetLength(0) ||
                        y < 0 || y >= tilePresence.GetLength(1))
                        continue;
                    
                    tilePresence[x, y] = true;
                }
            }
        }
    }
}