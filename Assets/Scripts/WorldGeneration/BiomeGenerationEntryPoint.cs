using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using WorldGeneration.Abstraction;
using WorldGeneration.RoomGeneration;

namespace WorldGeneration
{
    public class BiomeGenerationEntryPoint: SerializedMonoBehaviour
    {
        [SerializeField]
        private RoomData _roomData;
        
        [OdinSerialize]
        private IFirstStageGenerationStep _firstStep;
        
        [SerializeField]
        private bool _generateOnStart;
        
        [OdinSerialize]
        private ISecondStageGenerationStep _stageTwoFirstStep;
        
        public List<DoorInteractable> DoorInstances { get; } = new();

        private void Start()
        {
            if(_generateOnStart)
                GenerateRoom();
        }

        public void SetRoomData(RoomData roomData)
        {
            _roomData = roomData;
        }
        
        public void GenerateRoom()
        {
            if(_roomData == null)
                return;
            
            _firstStep.Generate(_roomData, null);
        }
        
        public void GenerateStageTwo(Room room, Room[,] tilePresence)
        {
            _stageTwoFirstStep.Generate(room, tilePresence);
        }
    }
}