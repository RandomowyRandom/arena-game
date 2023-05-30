using System;
using System.Collections.Generic;
using Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveSystem;
using WorldGeneration.Abstraction;
using WorldGeneration.RoomGeneration;
using static RoguelikeGeneratorPro.RoguelikeGeneratorPro.overlayType;
using static RoguelikeGeneratorPro.RoguelikeGeneratorPro.tileType;

namespace WorldGeneration
{
    [RequireComponent(typeof(RoguelikeGeneratorPro.RoguelikeGeneratorPro))]
    public class BiomeTerrainGenerator: SerializedMonoBehaviour, IGenerationStep
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
        
        public event Action<RoomData, bool[,]> OnGenerationComplete;

        private RoguelikeGeneratorPro.RoguelikeGeneratorPro _generator;

        private Vector2Int _fireplacePosition;
        private void Awake()
        {
            _generator = GetComponent<RoguelikeGeneratorPro.RoguelikeGeneratorPro>();
        }

        public void Generate(RoomData roomData, bool[,] tilePresence)
        {
            _generator.RigenenerateLevel();
            
            PlaceTiles();
            
            tilePresence = BiomeGenerationHelper.GetTilePresence(_wallTilemap);

            _fireplacePosition = BiomeGenerationHelper.FindLargestCircleCenter(tilePresence);
            
            if(_hasFireplace)
                tilePresence[_fireplacePosition.x, _fireplacePosition.y] = true;

            var isTilePresenceWidthOdd = tilePresence.GetLength(0) % 2 == 1;
            var fireplaceWorldPosition = BiomeGenerationHelper.GetWorldPositionFromOrigin(new Vector3Int(_fireplacePosition.x, _fireplacePosition.y, 0), transform, isTilePresenceWidthOdd);
            
            if (_hasFireplace)
            {
                Instantiate(_fireplacePrefab, fireplaceWorldPosition, Quaternion.identity, transform);
                BiomeGenerationHelper.DisableTilePresenceInRadius(tilePresence, _fireplacePosition, 15);
            }
            
            OnGenerationComplete?.Invoke(roomData, tilePresence);
            
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
    }
}