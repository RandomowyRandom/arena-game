using Items.RaritySystem;
using Player;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace UI.Inventory
{
    public class HotbarItemPreview: SerializedMonoBehaviour
    {
        [SerializeField]
        private TMP_Text _itemNameText;
        
        [OdinSerialize]
        private PlayerHotbarHandler _hotbarHandler;
        
        private void Start()
        {
            _hotbarHandler.OnUsableItemChanged += DisplayInfo;
        }
        
        private void OnDestroy()
        {
            _hotbarHandler.OnUsableItemChanged -= DisplayInfo;
        }

        private void DisplayInfo()
        {
            var item = _hotbarHandler.CurrentItem;
            
            if (item == null)
            {
                ClearInfo();
                return;
            }
            
            _itemNameText.SetText(item.ToString());
            
            var rarityData = item.GetAdditionalData<RarityAdditionalItemData>();
            
            _itemNameText.color = rarityData != null ? rarityData.GearRarity.Color : Color.white;
        }

        private void ClearInfo()
        {
            _itemNameText.SetText(string.Empty);
        }
    }
}