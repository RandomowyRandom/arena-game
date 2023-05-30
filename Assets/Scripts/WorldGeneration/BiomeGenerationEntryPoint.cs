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
        
        private void Start()
        {
            _firstStep.Generate(_roomData, null);
        }
    }
}