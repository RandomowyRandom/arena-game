using System;
using System.Collections.Generic;
using Common.Attributes;
using Items.ItemDataSystem;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Items
{
    [ScriptableFactoryElement]
    public class ItemDatabase: SerializedScriptableObject
    {
        [SerializeField]
        private List<ItemData> _itemDataList;
        
        [Space(10)]
        
        [OdinSerialize] [ReadOnly]
        private Dictionary<string, ItemData> _items;

        [Button]
        private void RefreshDatabase()
        {
            _items = new Dictionary<string, ItemData>();
            
            foreach (var itemData in _itemDataList)
            {
                _items.Add(itemData.Key, itemData);
            }
        }
        
        public ItemData GetItemData(string key)
        {
            RefreshDatabase();
            
            var item = _items[key];

            if (item != null) 
                return item;
            
            Debug.LogError($"Item with key {key} not found in database {name}.");
            throw new NotImplementedException();
        }
    }
}