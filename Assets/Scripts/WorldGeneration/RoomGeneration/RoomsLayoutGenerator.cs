using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WorldGeneration.RoomGeneration
{
    public class RoomsLayoutGenerator: SerializedMonoBehaviour
    {
        [SerializeField]
        private int _width;
        
        [SerializeField]
        private int _height;

        [SerializeField]
        private Vector2Int _startPosition;
        
        [SerializeField]
        private int _roomCount;
        
        [SerializeField] 
        private List<RoomData> _roomDatas;
        
        [Header("DEBUG")]
        [SerializeField]
        private TileBase _roomTile;
        
        [SerializeField]
        private TileBase _startingTile;
        
        [SerializeField]
        private Tilemap _tilemap;

        [Button]
        private void InstantiateTiles()
        {
            if (_tilemap == null || _roomTile == null || _startingTile == null)
            {
                Debug.LogError("Tilemap, room tile, or starting tile is not assigned!");
                return;
            }

            // Clear the tilemap
            _tilemap.ClearAllTiles();

            // Generate the room layout
            var roomsArray = Generate();

            // Iterate through the generated rooms
            for (var x = 0; x < _width; x++)
            {
                for (var y = 0; y < _height; y++)
                {
                    var room = roomsArray[x, y];
                    if (room == null)
                        continue;

                    // Instantiate the room tile
                    var position = new Vector3Int(room.X, room.Y, 0);
                    _tilemap.SetTile(position, _roomTile);

                    // Visualize open doors with debug draw rays
                    var openDoorSides = room.GetOpenDoorSides();
                    foreach (var openDoorSide in openDoorSides)
                    {
                        var doorPosition = position + GetOffsetFromOpenDoorSide(openDoorSide);
                        Debug.DrawRay(position + new Vector3(.5f, .5f), doorPosition - position, Color.green, 1f);
                    }

                    // Mark the start position with the starting tile
                    if (room.X == _startPosition.x && room.Y == _startPosition.y)
                    {
                        _tilemap.SetTile(position, _startingTile);
                    }
                }
            }
        }
        
       public Room[,] Generate()
{
    var roomsArray = new Room[_width, _height];
    var roomCount = 0;

    // Generate the starting room
    var startingRoom = new Room(_startPosition.x, _startPosition.y);
    roomsArray[_startPosition.x, _startPosition.y] = startingRoom;
    roomCount++;

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

            // Calculate the cost of generating a room at the adjacent position
            var cost = CalculateGenerationCost(roomsArray, neighborX, neighborY);

            // Create a new room at the adjacent position
            var newRoom = new Room(neighborX, neighborY);
            newRoom.AddOpenDoorSide(GetOppositeOpenDoorSide(openDoorSide));

            // Connect the new room to the open door of the current room
            currentRoom.AddOpenDoorSide(openDoorSide);

            // Add the new room to the array and enqueue it for further processing
            roomsArray[neighborX, neighborY] = newRoom;
            roomQueue.Enqueue(newRoom);

            roomCount++;
        }
    }

    return roomsArray;
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

        private int CalculateGenerationCost(Room[,] roomsArray, int x, int y)
        {
            var cost = 0;

            // Check left neighbor
            if (x > 0 && roomsArray[x - 1, y] != null)
            {
                cost++;
            }

            // Check right neighbor
            if (x < _width - 1 && roomsArray[x + 1, y] != null)
            {
                cost++;
            }

            // Check top neighbor
            if (y < _height - 1 && roomsArray[x, y + 1] != null)
            {
                cost++;
            }

            // Check bottom neighbor
            if (y > 0 && roomsArray[x, y - 1] != null)
            {
                cost++;
            }

            return cost;
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