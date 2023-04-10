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
        
        public Color Color => _color;
    }
}