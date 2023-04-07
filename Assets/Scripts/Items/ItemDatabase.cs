using System;
using System.Collections.Generic;
using Common.Attributes;
using Items.ItemDataSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Items
{
    [ScriptableFactoryElement]
    public class ItemDatabase: SerializedScriptableObject
    {
        [SerializeField]
        private List<ItemData> _items;
        
        private Dictionary<string, ItemData> _itemDictionary;
        
        private bool _isInitialized;
        
        public ItemData GetItemData(string key)
        {
            Initialize();
            
            var item = _itemDictionary[key];

            if (item != null) 
                return item;
            
            Debug.LogError($"Item with key {key} not found in database {name}.");
            throw new NotImplementedException();
        }
        
        private void Initialize()
        {
            if (_isInitialized)
                return;
            
            _itemDictionary = new Dictionary<string, ItemData>();
            
            foreach (var item in _items)
            {
                _itemDictionary.Add(item.Key, item);
            }
            
            _isInitialized = true;
        }
    }
}