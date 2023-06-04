using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using WorldGeneration.RoomGeneration;

namespace WorldGeneration
{
    public class WorldGeneratorManager: SerializedMonoBehaviour
    {
        [SerializeField]
        private BiomeGenerationEntryPoint _roomPrefab;

        [SerializeField]
        private int _width;
        
        [SerializeField]
        private int _height;
        
        [SerializeField]
        private int _roomCount;
        
        [SerializeField]
        private Vector2Int _startPosition;
        
        [SerializeField]
        private Vector2Int _startGenerationPosition;
        
        [SerializeField]
        private Vector2Int _roomSize;
        
        [OdinSerialize]
        private List<RoomData> _roomDatas;

        private RoomsLayoutGenerator _roomsLayoutGenerator;
        
        private Room[,] roomArray;
        
        [Button]
        private void Generate()
        {
            roomArray = _roomsLayoutGenerator.GetRoomArray();
            InstantiateRooms(roomArray, _roomSize);
        }

        private void Awake()
        {
            _roomsLayoutGenerator = new RoomsLayoutGenerator(_width, _height, _startPosition, _roomCount, _roomDatas);
        }

        private void InstantiateRooms(Room[,] rooms, Vector2 roomSize)
        {
            var width = rooms.GetLength(0);
            var height = rooms.GetLength(1);
            
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var room = rooms[x, y];
                    
                    if (room == null) 
                        continue;
                    
                    var roomPosition = new Vector2(_startGenerationPosition.x + x * roomSize.x, _startGenerationPosition.y + y * roomSize.y);
                    
                    var roomGameObject = Instantiate(_roomPrefab, roomPosition, Quaternion.identity);
                    
                    roomGameObject.SetRoomData(room.RoomData);

                    room.GenerationHandler = roomGameObject;
                }
            }

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var room = rooms[x, y];
                    
                    if (room == null) 
                        continue;

                    room.GenerationHandler.GenerateRoom();
                    room.GenerationHandler.GenerateStageTwo(room, rooms);
                    
                    if(!room.IsStartRoom)
                        room.GenerationHandler.gameObject.SetActive(false);
                }
            }

        }
    }
    

}