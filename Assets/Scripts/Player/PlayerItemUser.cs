using Items;
using Items.Abstraction;
using QFSW.QC;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

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

        [Command("use-item")]
        private void UseItemCommand()
        {
            if (_selectedItem is not UsableItem usableItem) 
                return;
            
            UseItem(usableItem);
        }

        #endregion
    }
}