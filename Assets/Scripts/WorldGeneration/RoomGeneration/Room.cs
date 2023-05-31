using System.Collections.Generic;

namespace WorldGeneration.RoomGeneration
{
    public class Room
    {
        public int X { get; set; }
        public int Y { get; set; }
        
        public RoomData RoomData { get; set; }
        
        private List<OpenDoorSide> _openDoorSides = new();
        
        public Room(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public void AddOpenDoorSide(OpenDoorSide openDoorSide)
        {
            _openDoorSides.Add(openDoorSide);
        }
        
        public List<OpenDoorSide> GetOpenDoorSides()
        {
            return new(_openDoorSides);
        }
    }
}