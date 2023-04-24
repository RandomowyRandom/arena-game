using System;
using Common.Attributes;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Items.RaritySystem
{
    [ScriptableFactoryElement]
    public class GearRarity: SerializedScriptableObject
    {
        [SerializeField]
        private Color _color = Color.white;
        
        [SerializeField]
        private Material _material;
        
        public Color Color => _color;
        
        public Material Material => _material;

        public override string ToString()
        {
            return name;
        }
    }
}