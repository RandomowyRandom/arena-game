using DG.Tweening;
using TMPro;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionTextHandler: MonoBehaviour
    {
        [SerializeField]
        private string _text;
        
        [SerializeField]
        private TMP_Text _textComponent;
        
        private Vector2 _initialPosition;
        
        private void Start()
        {
            _initialPosition = transform.localPosition;
            
            _textComponent.text = _text;
            Hide();
        }
        
        public void SetText(string text)
        {
            _text = text;
            _textComponent.text = _text;
        }

        public void Show()
        {
            if(this == null)
                return;
            
            transform.DOLocalMove( _initialPosition + Vector2.up * 0.5f, .18f);
            _textComponent.DOFade(1, .18f);
        }
        
        public void Hide()
        {
            if(this == null)
                return;
            
            transform.DOLocalMove(_initialPosition, .18f);
            _textComponent.DOFade(0, .18f);
        }
    }
}