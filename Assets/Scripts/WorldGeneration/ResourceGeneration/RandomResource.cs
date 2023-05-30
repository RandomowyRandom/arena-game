using System;
using UnityEngine;

namespace WorldGeneration
{
    [Serializable]
    public class RandomResource
    {
        [SerializeField]
        private GameObject _resourcePrefab;
        
        [SerializeField] [Range(0f, 1f)]
        private float _chance;
        
        public GameObject ResourcePrefab => _resourcePrefab;
        
        public float Chance => _chance;
    }
}