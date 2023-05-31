using System;
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
        private IGenerationStep _firstStep;
        
        [SerializeField]
        private bool _generateOnStart;

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
    }
}