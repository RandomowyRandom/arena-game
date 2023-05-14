using System;
using System.Collections.Generic;
using Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveSystem;

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
        
        [Header("Configs")]
        [SerializeField]
        private int _edgeRemovalSize = 1;
        
        public event Action<bool[,]> OnTerrainGenerated;

        public Vector2 BiomePosition { get; set; }
        public Vector2 BiomeSize => _generator.GetLevelSize();

        private const int TERRAIN_LAYER = 11;
        
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

        public void GenerateBiome()
        {
            _generator.RigenenerateLevel();

            CacheTilemaps();
            var tilePresence = BiomeGenerationHelper.GetTilePresence(_emptyTilemap, _wallTilemap);

            var wallTilemapRenderer = _wallTilemap.GetComponent<TilemapRenderer>();
            var floorTilemapRenderer = _floorTilemap.GetComponent<TilemapRenderer>();
            floorTilemapRenderer.sortingOrder = -2;
            wallTilemapRenderer.mode = TilemapRenderer.Mode.Individual;
            
            _fireplacePosition = BiomeGenerationHelper.FindLargestCircleCenter(tilePresence);
            
            tilePresence[_fireplacePosition.x, _fireplacePosition.y] = true;

            var fireplaceWorldPosition = BiomeGenerationHelper.GetWorldPositionFromTilemapPosition(new Vector3Int(_fireplacePosition.x, _fireplacePosition.y, 0), transform);
            
            if (_hasFireplace)
            {
                Instantiate(_fireplacePrefab, fireplaceWorldPosition, Quaternion.identity, transform);
                DisableTilePresenceInRadius(tilePresence, _fireplacePosition, 15);
            }

            MergeTilemaps(_floorTilemap, _overlayTilemap);
            MergeTilemaps(_wallTilemap, _emptyTilemap);
            
            RemoveEdgesFromTilemap(_wallTilemap, _edgeRemovalSize);
            
            _wallTilemap.gameObject.layer = TERRAIN_LAYER;

            transform.position = new Vector2(BiomePosition.x, BiomePosition.y);
            
            OnTerrainGenerated?.Invoke(tilePresence);
            
            if(_spawnPlayerHere)
                _playerEntity.transform.position = fireplaceWorldPosition - new Vector2(0, -2);
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