using System.Collections.Generic;

namespace WorldGeneration.RoomGeneration
{
    public class Room
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public bool IsStartRoom { get; set; }
        
        public RoomData RoomData { get; set; }
        
        private List<Door> _doors = new();
        
        public Room(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public void AddDoor(OpenDoorSide openDoorSide, int cost)
        {
            _doors.Add(new(openDoorSide, cost));
        }
        
        public List<Door> GetDoors()
        {
            return new(_doors);
        }
    }
}