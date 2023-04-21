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
        private string _key;
        
        public float MaxHealth => _maxHealth;
        
        public string Key => _key;
    }
}