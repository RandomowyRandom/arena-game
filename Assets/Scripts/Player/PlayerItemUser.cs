using Items;
using Items.Abstraction;
using Items.ItemDataSystem;
using JetBrains.Annotations;
using QFSW.QC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Player
{
    public class PlayerItemUser: SerializedMonoBehaviour, IItemUser
    {
        [OdinSerialize]
        private ItemData _selectedItem;
        
        public void UseItem(UsableItem item)
        {
            item.OnUse(this);
        }

        #region QC

        [Command("use-item")] [UsedImplicitly]
        private void UseItemCommand()
        {
            if (_selectedItem == null)
            {
                Debug.Log("No item selected");
                return;
            }
            
            if (_selectedItem is not UsableItem usableItem) 
                return;
            
            UseItem(usableItem);
        }

        #endregion
    }
}