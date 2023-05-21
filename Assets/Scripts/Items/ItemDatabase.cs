using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public List<ItemData> GetItemDataByQuery(Func<ItemData, bool> query)
        {
            RefreshDatabase();
            
            var items = new List<ItemData>();

            foreach (var item in _items.Values)
            {
                if (query(item))
                    items.Add(item);
            }

            return items;
        }

        public List<ItemData> GetItemData()
        {
            return new List<ItemData>(_items.Values.ToList());
        }
    }
}