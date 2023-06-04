using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using WorldGeneration.Abstraction;
using WorldGeneration.RoomGeneration;

namespace WorldGeneration
{
    public class BiomePredefinedWallHoles: SerializedMonoBehaviour, IFirstStageGenerationStep
    {
        [SerializeField]
        private List<Vector3Int> _wallHoles;
        
        [SerializeField]
        private Tilemap _wallTilemap;
        
        [OdinSerialize]
        private IFirstStageGenerationStep _previousStep;

        public event Action<RoomData, bool[,]> OnGenerationComplete;
        private void Awake()
        {
            _previousStep.OnGenerationComplete += Generate;
        }

        public void Generate(RoomData roomData, bool[,] tilePresence)
        {
            foreach (var hole in _wallHoles)
            {
                _wallTilemap.SetTile(hole, null);
            }
            
            OnGenerationComplete?.Invoke(roomData, tilePresence);
        }
    }
}