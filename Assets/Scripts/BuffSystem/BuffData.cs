﻿using System.Collections.Generic;
using BuffSystem.Abstraction;
using Common.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace BuffSystem
{
    [ScriptableFactoryElement]
    public class BuffData: SerializedScriptableObject
    {
        [OdinSerialize]
        private List<IBuffEffect> _effects;
        
        [OdinSerialize]
        private float _duration;
        
        [OdinSerialize]
        private Sprite _icon;
        
        public List<IBuffEffect> Effects => _effects;
        
        public float Duration => _duration;
        
        public Sprite Icon => _icon;
    }
}