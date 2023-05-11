using System;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveSystem;

namespace WorldGeneration
{
    [RequireComponent(typeof(RoguelikeGeneratorPro.RoguelikeGeneratorPro))]
    public class BiomeTerrainGenerator: MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField]
        private FireplaceController _fireplacePrefab;
        
        [Header("References")]
        [SerializeField]
        private PlayerEntity _playerEntity;
        
        public event Action<bool[,]> OnTerrainGenerated; 

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
            var tilePresence = BiomeGenerationHelper.GetTilePresence(_emptyTilemap, _wallTilemap);

            var wallTilemapRenderer = _wallTilemap.GetComponent<TilemapRenderer>();
            var floorTilemapRenderer = _floorTilemap.GetComponent<TilemapRenderer>();
            floorTilemapRenderer.sortingOrder = -2;
            wallTilemapRenderer.mode = TilemapRenderer.Mode.Individual;
            
            _fireplacePosition = BiomeGenerationHelper.FindLargestCircleCenter(tilePresence);
            
            tilePresence[_fireplacePosition.x, _fireplacePosition.y] = true;
            
            var fireplaceWorldPosition = BiomeGenerationHelper.GetWorldPositionFromTilemapPosition(new Vector3Int(_fireplacePosition.x, _fireplacePosition.y, 0), transform);
            Instantiate(_fireplacePrefab, fireplaceWorldPosition, Quaternion.identity);
            DisableTilePresenceInRadius(tilePresence, _fireplacePosition, 15);

            MergeTilemaps(_floorTilemap, _overlayTilemap);
            MergeTilemaps(_wallTilemap, _emptyTilemap);
            
            OnTerrainGenerated?.Invoke(tilePresence);
            
            // TODO: later move player teleportation to other class
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
    }
}