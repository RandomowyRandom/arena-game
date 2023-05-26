using System;
using DG.Tweening;
using Items.Abstraction;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class HotbarSlotUI: MonoBehaviour
    {
        [SerializeField]
        private Image _backgroundImage;
        
        [SerializeField]
        private Image _inUseImage;
        
        [Space(10)]
        [SerializeField]
        private Sprite _inactiveSlotSprite;
        
        [SerializeField]
        private Sprite _activeSlotSprite;
        
        private Vector2 _originalPosition = Vector2.zero;
        
        private bool _selected;
        
        public IItemUseLock ItemUseLock { get; set; }        

        public void SetSelected(bool selected)
        {
            _selected = selected;
            
            if(_originalPosition == Vector2.zero)
                _originalPosition = transform.position;
            
            transform.DOMove(_originalPosition, 0.2f);
            
            _backgroundImage.transform.localScale = Vector3.one;
            _backgroundImage.sprite = selected ? _activeSlotSprite : _inactiveSlotSprite;

            if (!selected) 
                return;
            
            transform.DOPunchScale(Vector3.one * 0.12f, 0.4f).onComplete +=
                () => transform.DOScale(Vector3.one, 0.2f);
            
            transform.DOMoveY(transform.position.y + 15f, 0.2f);
        }

        private void Update()
        {
            if(!_selected)
                return;

            _inUseImage.color = ItemUseLock.IsLocked ? new Color(0f, 0f, 0f, 0.5f) : Color.clear;
        }
    }
}