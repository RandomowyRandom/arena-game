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
        
        public void UseItem(UsableItem item)
        {
            item.OnUse(this);
        }

        private void Update()
        {
            if (!Mouse.current.leftButton.wasPressedThisFrame)
                return;
            
            var usableItem = _usableItemProvider.GetUsableItem();
            if(usableItem != null)
                UseItem(usableItem);
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

            UseItem(_usableItemProvider.GetUsableItem());
        }

        #endregion
    }
}