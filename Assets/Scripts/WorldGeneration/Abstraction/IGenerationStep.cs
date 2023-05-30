using System;
using WorldGeneration.RoomGeneration;

namespace WorldGeneration.Abstraction
{
    public interface IGenerationStep
    {
        public event Action<RoomData, bool[,]> OnGenerationComplete;
        
        public void Generate(RoomData roomData, bool[,] tilePresence);
    }
}