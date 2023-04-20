using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace UI
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
            _itemNameText.color = item.IsRarityItem ? item.GearRarity.Color : Color.white;
        }

        private void ClearInfo()
        {
            _itemNameText.SetText(string.Empty);
        }
    }
}