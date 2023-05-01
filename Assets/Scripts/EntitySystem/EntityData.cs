using Common.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem
{
    [ScriptableFactoryElement]
    public class EntityData : SerializedScriptableObject
    {
        [SerializeField]
        private float _maxHealth;
        
        [SerializeField]
        private int _requiredLevel = 0;
        
        [SerializeField]
        private bool _isStatic;
        
        public float MaxHealth => _maxHealth;
        
        public int RequiredLevel => _requiredLevel;
        
        public string Key => name;
        
        public bool IsStatic => _isStatic;
    }
}