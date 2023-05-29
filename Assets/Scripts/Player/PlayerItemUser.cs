using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Items;
using Items.Abstraction;
using Items.ItemDataSystem;
using JetBrains.Annotations;
using Player.Interfaces;
using QFSW.QC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerItemUser: SerializedMonoBehaviour, IItemUser, IItemUseLock
    {
        [OdinSerialize]
        private IUsableItemProvider _usableItemProvider;
        
        [OdinSerialize]
        private List<IItemUseLock> _itemUseLocks;
        
        [SerializeField]
        private GameObject _parentGameObject;
        
        public bool IsLocked => _useLock || _useDelay > 0;

        public GameObject GameObject => gameObject;
        public GameObject ParentGameObject => _parentGameObject;

        private float _useDelay;
        
        private bool _useLock;

        private void Update()
        {
            _useDelay -= Time.deltaTime;
            
            if (!Mouse.current.leftButton.isPressed)
                return;
            
            var usableItem = _usableItemProvider.GetUsableItem();
            if(usableItem != null)
                TryUseItem(usableItem).Forget();
        }
        
        public async UniTask<bool> TryUseItem(UsableItem item)
        {
            if (_itemUseLocks.Any(l => l.IsLocked))
                return false;

            _useLock = true;
            await item.OnUse(this);
            _useLock = false;
            
            _useDelay = ServiceLocator.ServiceLocator.Instance.Get<IPlayerStats>().GetStatsData().FireRate;
            return true;
        }

        public void ConsumeItem(UsableItem item)
        {
            _usableItemProvider.ConsumeItem(item);
        }

        public void DecreaseDurability(UsableItem item, int amount)
        {
            _usableItemProvider.DecreaseDurability(item, amount);
        }
    }
}