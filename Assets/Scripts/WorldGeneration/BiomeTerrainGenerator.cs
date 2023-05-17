using System;
using System.Collections.Generic;
using Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveSystem;
using static RoguelikeGeneratorPro.RoguelikeGeneratorPro.overlayType;
using static RoguelikeGeneratorPro.RoguelikeGeneratorPro.tileType;

namespace WorldGeneration
{
    [RequireComponent(typeof(RoguelikeGeneratorPro.RoguelikeGeneratorPro))]
    public class BiomeTerrainGenerator: SerializedMonoBehaviour
    {
        [Header("Prefabs")]
        [OdinSerialize]
        private bool _hasFireplace;
        [OdinSerialize] [ShowIf(nameof(_hasFireplace))]
        private FireplaceController _fireplacePrefab;
        
        [Header("References")]
        [OdinSerialize]
        private bool _spawnPlayerHere;
        [OdinSerialize] [ShowIf(nameof(_spawnPlayerHere))]
        private PlayerEntity _playerEntity;
        
        [Header("Tiles")]
        [OdinSerialize]
        private TileBase _wallTile;
        
        [OdinSerialize]
        private TileBase _floorTile;
        
        [OdinSerialize]
        private TileBase _overlayTile;
        
        [Header("Tilemaps")]
        
        [SerializeField]
        private Tilemap _wallTilemap;

        [SerializeField]
        private Tilemap _floorTilemap;

        [SerializeField]
        private Tilemap _overlayTilemap;
        
        [Header("Configs")]
        [SerializeField]
        private int _edgeRemovalSize = 1;
        
        public event Action<bool[,]> OnTerrainGenerated;

        public Vector2 BiomePosition { get; set; }
        public Vector2 BiomeSize => _generator.GetLevelSize();
        private RoguelikeGeneratorPro.RoguelikeGeneratorPro _generator;

        private Vector2Int _fireplacePosition;
        private void Awake()
        {
            _generator = GetComponent<RoguelikeGeneratorPro.RoguelikeGeneratorPro>();
        }

        public void GenerateBiome()
        {
            _generator.RigenenerateLevel();
            
            PlaceTiles();
            
            var tilePresence = BiomeGenerationHelper.GetTilePresence(_wallTilemap);

            _fireplacePosition = BiomeGenerationHelper.FindLargestCircleCenter(tilePresence);
            
            tilePresence[_fireplacePosition.x, _fireplacePosition.y] = true;

            var fireplaceWorldPosition = BiomeGenerationHelper.GetWorldPositionFromTilemapPosition(new Vector3Int(_fireplacePosition.x, _fireplacePosition.y, 0), transform);
            
            if (_hasFireplace)
            {
                Instantiate(_fireplacePrefab, fireplaceWorldPosition, Quaternion.identity, transform);
                DisableTilePresenceInRadius(tilePresence, _fireplacePosition, 15);
            }

            RemoveEdgesFromTilemap(_wallTilemap, _edgeRemovalSize);
            
            transform.position = new Vector2(BiomePosition.x, BiomePosition.y);
            
            OnTerrainGenerated?.Invoke(tilePresence);
            
            if(_spawnPlayerHere)
                _playerEntity.transform.position = fireplaceWorldPosition - new Vector2(0, -2);
        }

        private void PlaceTiles()
        {
            var tiles = _generator.GetTiles();
            var overlay = _generator.GetOverlayTiles();

            var wallTiles = new List<PositionTile>();
            var floorTiles = new List<PositionTile>();
            var overlayTiles = new List<PositionTile>();

            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                for (var y = 0; y < tiles.GetLength(1); y++)
                {
                    var tile = tiles[x, y];
                    var overlayTile = overlay[x, y];
                    if(tile is wall or RoguelikeGeneratorPro.RoguelikeGeneratorPro.tileType.empty)
                        wallTiles.Add(new PositionTile
                        {
                            Position = new Vector2Int(x, y),
                            Tile = tile
                        });
                    
                    if(overlayTile is floorRandom or floorPattern)
                        overlayTiles.Add(new PositionTile
                        {
                            Position = new Vector2Int(x, y),
                            Tile = floor
                        });
                    
                    floorTiles.Add(new PositionTile
                    {
                        Position = new Vector2Int(x, y),
                        Tile = floor
                    });
                }
            }
            
            _wallTilemap.SetTiles
                (wallTiles.ConvertAll(
                    tile => new Vector3Int(tile.Position.x, tile.Position.y, 0))
                    .ToArray(), 
                    wallTiles.ConvertAll(_ => _wallTile)
                     .ToArray());

            _floorTilemap.SetTiles(
                floorTiles.ConvertAll(
                    tile => new Vector3Int(tile.Position.x, tile.Position.y, 0))
                    .ToArray(), 
                floorTiles.ConvertAll(_ => _floorTile)
                    .ToArray());
            
            _overlayTilemap.SetTiles(
                overlayTiles.ConvertAll(
                    tile => new Vector3Int(tile.Position.x, tile.Position.y, 0))
                    .ToArray(),
                overlayTiles.ConvertAll(_ => _overlayTile)
                    .ToArray());
        }


        private void DisableTilePresenceInRadius(bool[,] tilePresence, Vector2Int center, int radius)
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

        private void RemoveEdgesFromTilemap(Tilemap tilemap, int edgeThickness = 1)
        {
            var cellBounds = tilemap.cellBounds;
            var edges = new List<Vector3Int>();

            for (var x = cellBounds.xMin; x < cellBounds.xMax; x++)
            {
                for (var y = cellBounds.yMin; y < cellBounds.yMax; y++)
                {
                    var tilePos = new Vector3Int(x, y, 0);
                    var tile = tilemap.GetTile(tilePos);
                    
                    if(tile == null)
                        continue;
                    
                    if (x < cellBounds.xMin + edgeThickness || x >= cellBounds.xMax - edgeThickness ||
                        y < cellBounds.yMin + edgeThickness || y >= cellBounds.yMax - edgeThickness)
                    {
                        edges.Add(tilePos);
                    }
                }
            }
            
            tilemap.SetTiles(edges.ToArray(), new TileBase[edges.Count]);
        }
    }
}