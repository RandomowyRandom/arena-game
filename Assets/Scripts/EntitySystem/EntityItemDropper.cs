using System;
using Items;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace EntitySystem
{
    [RequireComponent(typeof(Entity))]
    public class EntityItemDropper: SerializedMonoBehaviour
    {
        [OdinSerialize]
        private DropTable _dropTable;
        
        [SerializeField]
        private ItemWorld _itemWorldPrefab;
        
        private Entity _entity;

        private void Awake()
        {
            _entity = GetComponent<Entity>();
            _entity.OnDeath += DropItems;
        }

        private void OnDestroy()
        {
            _entity.OnDeath -= DropItems;
        }

        private void DropItems()
        {
            var drops = _dropTable.GetDrops();

            foreach (var drop in drops)
            {
                var item = Instantiate(_itemWorldPrefab, transform.position, Quaternion.identity);
                item.SetItem(drop);
            }
        }
    }
}