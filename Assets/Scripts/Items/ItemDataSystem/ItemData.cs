using System;
using Common.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Items.ItemDataSystem
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
        
        [SerializeField]
        private int _requiredLevel;

        public string Key => name;
        
        public string DisplayName
        {
            get => _displayName;
            set => _displayName = value;
        }

        public Sprite Icon
        {
            get => _icon;
            set => _icon = value;
        }

        public string Description
        {
            get => _description;
            set => _description = value;
        }

        public int MaxStack
        {
            get => _maxStack;
            set => _maxStack = value;
        }

        public bool IsStackable => _maxStack > 1;
        
        public int RequiredLevel
        {
            get => _requiredLevel;
            set => _requiredLevel = value;
        }

        public virtual void OnItemConstructed(Item item) { }
        
        public override string ToString()
        {
            return Key;
        }
    }
}