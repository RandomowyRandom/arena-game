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

        [SerializeField] 
        private EntityAttackerGroup _attackerGroup;
        
        private float _currentMaxHealth;

        protected override void OnAfterDeserialize()
        {
            _currentMaxHealth = _maxHealth;
        }

        public float MaxHealth
        {
            get => _currentMaxHealth;
            set => _currentMaxHealth = value;
        }

        public int RequiredLevel => _requiredLevel;
        
        public string Key => name;
        
        public bool IsStatic => _isStatic;
        
        public EntityAttackerGroup AttackerGroup => _attackerGroup;
    }
}