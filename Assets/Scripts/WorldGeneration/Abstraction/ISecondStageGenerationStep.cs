using System;
using WorldGeneration.RoomGeneration;

namespace WorldGeneration.Abstraction
{
    public interface ISecondStageGenerationStep
    {
        public event Action<Room, Room[,]> OnGenerationComplete;
        
        public void Generate(Room room, Room[,] rooms);
    }
}