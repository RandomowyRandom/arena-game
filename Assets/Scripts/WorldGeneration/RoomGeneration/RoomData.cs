using System.Collections.Generic;
using Common.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace WorldGeneration.RoomGeneration
{
    [ScriptableFactoryElement]
    public class RoomData: SerializedScriptableObject
    {
        [SerializeField] [Range(1, 4)] 
        private int _level;
        
        [SerializeField]
        private bool _generateTreasure;
        
        [OdinSerialize] [ShowIf(nameof(_generateTreasure))]
        private GameObject _treasurePrefab;
        
        [Space(30)]
        
        [SerializeField]
        private bool _generateResources;
        
        [OdinSerialize] [ShowIf(nameof(_generateResources))]
        private int _randomPointCount;

        [OdinSerialize] [Range(0f, 1f)] [ShowIf(nameof(_generateResources))]
        private float _percentageOfAllRandomPoints;
        
        [OdinSerialize] [ShowIf(nameof(_generateResources))]
        private List<ResourceGroupData> _resourceGroups;

        public int Level => _level;
        
        public bool GenerateTreasure => _generateTreasure;

        public GameObject TreasurePrefab => _treasurePrefab;

        public bool GenerateResources => _generateResources;

        public int RandomPointCount => _randomPointCount;

        public float PercentageOfAllRandomPoints => _percentageOfAllRandomPoints;

        public List<ResourceGroupData> ResourceGroups => _resourceGroups;
    }
}