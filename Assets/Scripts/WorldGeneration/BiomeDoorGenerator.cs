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
        [SerializeField]
        private Tilemap _wallTilemap;

        [SerializeField]
        private Vector2Int _roomSize;

        [SerializeField] private GameObject _doorPrefab; // TODO: later change to DoorWorld class

        public event Action<Room, Room[,]> OnGenerationComplete;

        public void Generate(Room room, Room[,] rooms)
        {
            var openDoors = room.GetDoors();

            if (room.IsStartRoom)
                openDoors.Add(new(OpenDoorSide.Bottom, 1));
                
            foreach (var openDoor in openDoors)
            {
                var doorPositions = GetDoorTilePositions(openDoor.OpenDoorSide);
                foreach (var doorPosition in doorPositions)
                {
                    _wallTilemap.SetTile(doorPosition, null);
                }
                
                var doorInstancePosition = GetDoorWorldPosition(openDoor);
                var doorInstance = Instantiate(_doorPrefab, doorInstancePosition, Quaternion.identity);
            }
            
            OnGenerationComplete?.Invoke(room, rooms);
        }

        private Vector2 GetDoorWorldPosition(Door door)
        {
            return door.OpenDoorSide switch
            {
                OpenDoorSide.Left => new(transform.position.x, transform.position.y + _roomSize.y * .5f),
                OpenDoorSide.Right => new(transform.position.x + _roomSize.x, transform.position.y + _roomSize.y * .5f),
                OpenDoorSide.Top => new(transform.position.x + _roomSize.x * .5f + .5f, transform.position.y + _roomSize.y - .5f),
                OpenDoorSide.Bottom => new(transform.position.x + _roomSize.x * .5f + .5f, transform.position.y + .5f),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private List<Vector3Int> GetDoorTilePositions(OpenDoorSide side)
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