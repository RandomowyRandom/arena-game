using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HotbarSlotUI: MonoBehaviour
    {
        [SerializeField]
        private Image _backgroundImage;
        
        [Space(10)]
        [SerializeField]
        private Sprite _inactiveSlotSprite;
        
        [SerializeField]
        private Sprite _activeSlotSprite;
        
        private Vector2 _originalPosition = Vector2.zero;
        

        public void SetSelected(bool selected)
        {
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
    }
}