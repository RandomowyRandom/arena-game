using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using WorldGeneration.Abstraction;
using WorldGeneration.RoomGeneration;

namespace WorldGeneration
{
    public class BiomeDoorGenerator : SerializedMonoBehaviour, ISecondStageGenerationStep
    {
        [SerializeField] private Tilemap _wallTilemap;

        [SerializeField] private Vector2Int _roomSize;

        public event Action<Room, Room[,]> OnGenerationComplete;

        public void Generate(Room room, Room[,] rooms)
        {
            var openDoors = room.GetOpenDoorSides();

            if (room.IsStartRoom)
                openDoors.Add(OpenDoorSide.Bottom);
                
            foreach (var openDoor in openDoors)
            {
                var doorPositions = GetDoorPositions(openDoor);
                foreach (var doorPosition in doorPositions)
                {
                    _wallTilemap.SetTile(doorPosition, null);
                }
            }
            
            OnGenerationComplete?.Invoke(room, rooms);
        }

        private List<Vector3Int> GetDoorPositions(OpenDoorSide side)
        {
            return side switch
            {
                OpenDoorSide.Left => new List<Vector3Int>()
                {
                    new(0, Mathf.CeilToInt((float)_roomSize.y / 2), 0),
                    new(0, Mathf.FloorToInt((float)_roomSize.y / 2), 0)
                },
                OpenDoorSide.Right => new List<Vector3Int>()
                {
                    new(_roomSize.x - 1, Mathf.CeilToInt((float)_roomSize.y / 2), 0),
                    new(_roomSize.x - 1, Mathf.FloorToInt((float)_roomSize.y / 2), 0)
                },
                OpenDoorSide.Top => new List<Vector3Int>()
                {
                    new(Mathf.CeilToInt((float)_roomSize.x / 2), _roomSize.y - 1, 0),
                    new(Mathf.FloorToInt((float)_roomSize.x / 2), _roomSize.y - 1, 0)
                },
                OpenDoorSide.Bottom => new List<Vector3Int>()
                {
                    new(Mathf.CeilToInt((float)_roomSize.x / 2), 0, 0),
                    new(Mathf.FloorToInt((float)_roomSize.x / 2), 0, 0)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }
    }
}