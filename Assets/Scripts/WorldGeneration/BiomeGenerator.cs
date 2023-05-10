using System;
using Player;
using QFSW.QC;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveSystem;

namespace WorldGeneration
{
    [RequireComponent(typeof(RoguelikeGeneratorPro.RoguelikeGeneratorPro))]
    public class BiomeGenerator: MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private FireplaceController _fireplacePrefab;
        
        [Header("References")]
        [SerializeField]
        private PlayerEntity _playerEntity;
        
        private RoguelikeGeneratorPro.RoguelikeGeneratorPro _generator;
        
        private Tilemap _emptyTilemap;
        private Tilemap _wallTilemap;
        private Tilemap _overlayTilemap;
        private Tilemap _floorTilemap;
        
        private Vector2Int _fireplacePosition;
        private void Awake()
        {
            _generator = GetComponent<RoguelikeGeneratorPro.RoguelikeGeneratorPro>();
        }

        private void Start()
        {
            GenerateBiome();
        }

        private void GenerateBiome()
        {
            _generator.RigenenerateLevel();

            CacheTilemaps();
            var tilePresence = GetTilePresence(_emptyTilemap, _wallTilemap);

            _floorTilemap.GetComponent<Renderer>().sortingOrder = -1;
            
            _fireplacePosition = FindLargestCircleCenter(tilePresence);

            Debug.Log(_fireplacePosition);
            
            var fireplaceWorldPosition = GetWorldPositionFromTilemapPosition(new Vector3Int(_fireplacePosition.x, _fireplacePosition.y, 0));
            Instantiate(_fireplacePrefab, fireplaceWorldPosition, Quaternion.identity);

            MergeTilemaps(_floorTilemap, _overlayTilemap);
            MergeTilemaps(_wallTilemap, _emptyTilemap);
            
            _playerEntity.transform.position = fireplaceWorldPosition - new Vector2(0, -2);
        }

        private void CacheTilemaps()
        {
            // this is a bit of a hack, but generator each time creates a new tilemap, so we need to find it
            _emptyTilemap = transform.GetChild(0).GetChild(2).GetComponent<Tilemap>();
            _wallTilemap = transform.GetChild(0).GetChild(1).GetComponent<Tilemap>();
            _overlayTilemap = transform.GetChild(0).GetChild(3).GetComponent<Tilemap>();
            _floorTilemap = transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        }

        private void MergeTilemaps(Tilemap margeTo, Tilemap mergeFrom, bool destroyMergeFrom = true)
        {
            var cellBounds = mergeFrom.cellBounds;
            for (var x = cellBounds.xMin; x < cellBounds.xMax; x++)
            {
                for (var y = cellBounds.yMin; y < cellBounds.yMax; y++)
                {
                    var tilePos = new Vector3Int(x, y, 0);
                    var tile = mergeFrom.GetTile(tilePos);
                    
                    if(tile == null)
                        continue;
                    
                    margeTo.SetTile(tilePos, tile);
                }
            }
            
            if(destroyMergeFrom)
                Destroy(mergeFrom.gameObject);
            else
                mergeFrom.ClearAllTiles();
        }
        
        private bool[,] GetTilePresence(Tilemap tilemap1, Tilemap tilemap2)
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
        
        private Vector2Int FindLargestCircleCenter(bool[,] grid) {
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
        
        private Vector2 GetWorldPositionFromTilemapPosition(Vector3Int tilemapPosition)
        {
            return new Vector2(tilemapPosition.x + .5f, tilemapPosition.y + .5f)
                   + (Vector2)transform.position;
        }
        
        private float FindLargestCircleRadius(int x, int y, bool[,] grid) {
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

    }
}