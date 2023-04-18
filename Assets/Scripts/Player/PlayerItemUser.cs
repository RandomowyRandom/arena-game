using System;
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
    public class PlayerItemUser: SerializedMonoBehaviour, IItemUser
    {
        [OdinSerialize]
        private IUsableItemProvider _usableItemProvider;
        
        public GameObject GameObject => gameObject;
        
        private float _useDelay;
        
        public bool TryUseItem(UsableItem item)
        {
            if(_useDelay > 0)
                return false;
            
            item.OnUse(this);

            _useDelay = ServiceLocator.ServiceLocator.Instance.Get<IPlayerStats>().GetStatsData().FireRate;
            return true;
        }

        private void Update()
        {
            _useDelay -= Time.deltaTime;
            
            if (!Mouse.current.leftButton.isPressed)
                return;
            
            var usableItem = _usableItemProvider.GetUsableItem();
            if(usableItem != null)
                TryUseItem(usableItem);
        }

        #region QC

        [Command("use-item")] [UsedImplicitly]
        private void UseItemCommand()
        {
            if (_usableItemProvider.GetUsableItem() == null)
            {
                Debug.Log("No item selected");
                return;
            }

            TryUseItem(_usableItemProvider.GetUsableItem());
        }

        #endregion
    }
}