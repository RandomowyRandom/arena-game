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
        
        [SerializeField] 
        private DoorInteractable _doorPrefab;

        [Header("Door transforms")]
        [SerializeField]
        private Transform _leftDoorTransform;

        [SerializeField]
        private Transform _rightDoorTransform;
        
        [SerializeField]
        private Transform _topDoorTransform;
        
        [SerializeField]
        private Transform _bottomDoorTransform;

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
                
                var doorInstanceTransform = GetDoorTransform(openDoor);

                var doorInstance = Instantiate(_doorPrefab, doorInstanceTransform.position, doorInstanceTransform.rotation, transform);
                var targetRoom = GetRoomAtSide(openDoor.OpenDoorSide, new Vector2Int(room.X, room.Y), rooms);
                
                if(targetRoom != null)
                    doorInstance.TargetRoom = targetRoom.GenerationHandler;
                
                if(room.IsStartRoom && openDoor.OpenDoorSide == OpenDoorSide.Bottom)
                    doorInstance.TargetRoom = room.GenerationHandler;
                
                room.GenerationHandler.DoorInstances.Add(doorInstance);
                
                doorInstance.ParentRoom = room.GenerationHandler;
                doorInstance.Door = openDoor;
            }
            
            OnGenerationComplete?.Invoke(room, rooms);
        }

        private Room GetRoomAtSide(OpenDoorSide side, Vector2Int position, Room[,] rooms)
        {
            return side switch
            {
                OpenDoorSide.Left => rooms[position.x - 1, position.y],
                OpenDoorSide.Right => rooms[position.x + 1, position.y],
                OpenDoorSide.Top => rooms[position.x, position.y + 1],
                OpenDoorSide.Bottom => position.y - 1 >= 0 ? rooms[position.x, position.y - 1] : null,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, null)
            };
        }
        
        private Transform GetDoorTransform(Door door)
        {
            return door.OpenDoorSide switch
            {
                OpenDoorSide.Left => _leftDoorTransform,
                OpenDoorSide.Right => _rightDoorTransform,
                OpenDoorSide.Top => _topDoorTransform,
                OpenDoorSide.Bottom => _bottomDoorTransform,
                _ => null
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