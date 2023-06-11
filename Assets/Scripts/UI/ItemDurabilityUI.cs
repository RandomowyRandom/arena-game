using System;
using Items.Durability;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ItemDurabilityUI: MonoBehaviour
    {
        [SerializeField]
        private PlayerHotbarHandler _playerHotbarHandler;
        
        [SerializeField]
        private TMP_Text _durabilityText;

        private void Start()
        {
            _playerHotbarHandler.OnUsableItemChanged += UpdateUI;
        }

        private void UpdateUI()
        {
            var currentItem = _playerHotbarHandler.CurrentItem;

            if (currentItem == null)
            {
                _durabilityText.SetText(string.Empty);
                return;
            }
            
            var durabilityItemData = currentItem.GetAdditionalData<DurabilityItemData>();

            if(durabilityItemData == null)
                return;
            
            SetDurabilityUI(durabilityItemData.GetTooltip());
        }

        private void SetDurabilityUI(string text)
        {
            _durabilityText.SetText(text);
        }
    }
}