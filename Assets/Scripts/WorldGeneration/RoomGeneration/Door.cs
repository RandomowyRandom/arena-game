namespace WorldGeneration.RoomGeneration
{
    public class Door
    {
        public OpenDoorSide OpenDoorSide { get; set; }
        
        public int OpenCost { get; set; }
        
        public Door(OpenDoorSide openDoorSide, int openCost)
        {
            OpenDoorSide = openDoorSide;
            OpenCost = openCost;
        }
    }
}