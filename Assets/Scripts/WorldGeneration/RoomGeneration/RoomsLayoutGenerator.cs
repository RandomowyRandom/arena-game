using System;
using System.Collections.Generic;
using Common.Extensions;
using UnityEngine;

namespace WorldGeneration.RoomGeneration
{
    public class RoomsLayoutGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Vector2Int _startPosition;
        private readonly int _roomCount;
        private readonly List<RoomData> _roomDatas;
        
        public RoomsLayoutGenerator(int width, int height, Vector2Int startPosition, int roomCount, List<RoomData> roomDatas)
        {
            _width = width;
            _height = height;
            _startPosition = startPosition;
            _roomCount = roomCount;
            _roomDatas = roomDatas;
        }
        
        public Room[,] GetRoomArray()
        {
            var roomsArray = new Room[_width, _height];
            var roomCount = 0;

            // Generate the starting room
            var startingRoom = new Room(_startPosition.x, _startPosition.y)
            {
                IsStartRoom = true
            };
            roomsArray[_startPosition.x, _startPosition.y] = startingRoom;
            roomCount++;

            startingRoom.RoomData = GetRandomRoomData(1);
            
            // Create a queue to store the rooms that need to be processed
            var roomQueue = new Queue<Room>();
            roomQueue.Enqueue(startingRoom);

            // Repeat until the room count is reached or the queue becomes empty
            while (roomQueue.Count > 0 && roomCount < _roomCount)
            {
                var currentRoom = roomQueue.Dequeue();

                // Generate random open doors for the room
                var openDoorSides = GetRandomOpenDoorSides();

                foreach (var openDoorSide in openDoorSides)
                {
                    var neighborX = currentRoom.X;
                    var neighborY = currentRoom.Y;

                    // Calculate the position of the adjacent room based on the open door side
                    switch (openDoorSide)
                    {
                        case OpenDoorSide.Left:
                            neighborX--;
                            break;
                        case OpenDoorSide.Right:
                            neighborX++;
                            break;
                        case OpenDoorSide.Top:
                            neighborY++;
                            break;
                        case OpenDoorSide.Bottom:
                            neighborY--;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    // Check if the adjacent position is within the bounds of the array
                    if (neighborX < 0 || neighborX >= _width || neighborY < 0 || neighborY >= _height)
                        continue;

                    // Check if there is no room already present at the adjacent position
                    if (roomsArray[neighborX, neighborY] != null)
                        continue;

                    // Determine the level of the room based on the generation progress
                    var level = Math.Min(roomCount / (_roomCount / 4) + 1, 4);
                    
                    // Create a new room at the adjacent position
                    var newRoom = new Room(neighborX, neighborY);
                    newRoom.AddDoor(GetOppositeOpenDoorSide(openDoorSide), level);

                    newRoom.RoomData = GetRandomRoomData(level); // Assign a random RoomData based on the level
                    
                    // Connect the new room to the open door of the current room
                    currentRoom.AddDoor(openDoorSide, currentRoom.RoomData.Level);

                    // Add the new room to the array and enqueue it for further processing
                    roomsArray[neighborX, neighborY] = newRoom;
                    roomQueue.Enqueue(newRoom);

                    roomCount++;
                }
            }

            return roomsArray;
        }

        private RoomData GetRandomRoomData(int level)
        {
            var shuffledRooms = new List<RoomData>(_roomDatas);
            shuffledRooms.Shuffle();
            
            foreach (var roomData in shuffledRooms)
            {
                if (roomData.Level == level)
                    return roomData;
            }
            
            return null;
        }

        private List<OpenDoorSide> GetRandomOpenDoorSides()
        {
            var openDoorSides = new List<OpenDoorSide>();

            // Generate a random number of open doors between 2 and 4
            var numOpenDoors = UnityEngine.Random.Range(2, 5);

            // Create a list of all possible open door sides
            var allOpenDoorSides = new List<OpenDoorSide>
            {
                OpenDoorSide.Left,
                OpenDoorSide.Right,
                OpenDoorSide.Top,
                OpenDoorSide.Bottom
            };

            // Shuffle the list of open door sides
            for (var i = 0; i < allOpenDoorSides.Count; i++)
            {
                var randomIndex = UnityEngine.Random.Range(i, allOpenDoorSides.Count);
                (allOpenDoorSides[randomIndex], allOpenDoorSides[i]) = (allOpenDoorSides[i], allOpenDoorSides[randomIndex]);
            }

            // Add the first 'numOpenDoors' open door sides to the result list
            for (var i = 0; i < numOpenDoors; i++)
            {
                openDoorSides.Add(allOpenDoorSides[i]);
            }

            return openDoorSides;
        }

        private OpenDoorSide GetOppositeOpenDoorSide(OpenDoorSide openDoorSide)
        {
            return openDoorSide switch
            {
                OpenDoorSide.Left => OpenDoorSide.Right,
                OpenDoorSide.Right => OpenDoorSide.Left,
                OpenDoorSide.Top => OpenDoorSide.Bottom,
                OpenDoorSide.Bottom => OpenDoorSide.Top,
                _ => throw new ArgumentException("Invalid open door side")
            };
        }
        
        private Vector3Int GetOffsetFromOpenDoorSide(OpenDoorSide openDoorSide)
        {
            return openDoorSide switch
            {
                OpenDoorSide.Left => Vector3Int.left,
                OpenDoorSide.Right => Vector3Int.right,
                OpenDoorSide.Top => Vector3Int.up,
                OpenDoorSide.Bottom => Vector3Int.down,
                _ => Vector3Int.zero
            };
        }
    }
}