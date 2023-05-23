using System;
using DG.Tweening;
using PlayerUpgradeSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeUI: MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private Image _image;
        
        [SerializeField]
        private TMP_Text _descriptionText;

        public event Action<UpgradeUI> OnSelected;

        private PlayerUpgrade _upgrade;
        
        public PlayerUpgrade Upgrade => _upgrade;
        
        public void SetUpgrade(PlayerUpgrade upgrade)
        {
            _upgrade = upgrade;
            
            _image.sprite = upgrade.Icon;
            _descriptionText.text = upgrade.Description;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button != PointerEventData.InputButton.Left)
                return;
            
            OnSelected?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one * 1.2f, .2f).timeScale = 1;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one, .2f).timeScale = 1;
        }
    }
}