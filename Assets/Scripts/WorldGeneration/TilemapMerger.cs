using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration
{
    public class TilemapMerger: SerializedMonoBehaviour
    {
        [SerializeField]
        private Tilemap _mergeFrom;
        
        [SerializeField]
        private bool _useTilemapName;
        
        [SerializeField] [HideIf(nameof(_useTilemapName))]
        private Tilemap _mergeTo;
        
        [SerializeField] [ShowIf(nameof(_useTilemapName))]
        private string _mergeToName;

        public void Merge()
        {
            if (_mergeTo == null)
                _mergeTo = GameObject.Find(_mergeToName).GetComponent<Tilemap>();

            var fromBounds = _mergeFrom.cellBounds;
            var toBounds = _mergeTo.cellBounds;

            var tilePositions = new List<Vector3Int>();
            var tiles = new List<TileBase>();

            for (var x = fromBounds.xMin; x < fromBounds.xMax; x++)
            {
                for (var y = fromBounds.yMin; y < fromBounds.yMax; y++)
                {
                    var tile = _mergeFrom.GetTile(new Vector3Int(x, y, 0));
                    if (tile == null)
                        continue;

                    var tilePosition = new Vector3Int(x, y, 0);
                    var tileWorldPosition = _mergeFrom.CellToWorld(tilePosition);
                    var tileLocalPosition = _mergeTo.WorldToCell(tileWorldPosition);
                    var tileLocalPositionWithOffset = tileLocalPosition - toBounds.min;

                    tilePositions.Add(tileLocalPositionWithOffset);
                    tiles.Add(tile);
                }
            }

            _mergeTo.SetTiles(tilePositions.ToArray(), tiles.ToArray());
            
            Destroy(_mergeFrom.gameObject);
        }
    }
}