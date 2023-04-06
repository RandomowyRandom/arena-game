using Common.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Items
{
    [ScriptableFactoryElement]
    public class ItemData: SerializedScriptableObject
    {
        [SerializeField]
        private string _displayName;
        
        [SerializeField]
        private Sprite _icon;
        
        [SerializeField]
        private string _description;
        
        [SerializeField]
        private int _maxStack;

        public string DisplayName => _displayName;

        public Sprite Icon => _icon;

        public string Description => _description;

        public int MaxStack => _maxStack;
        
        public bool IsStackable => _maxStack > 1;
    }
}