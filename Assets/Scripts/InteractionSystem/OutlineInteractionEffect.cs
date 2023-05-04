using System;
using UnityEngine;

namespace InteractionSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class OutlineInteractionEffect: MonoBehaviour
    {
        private readonly int _outlinePixelWidth = Shader.PropertyToID("_OutlinePixelWidth");

        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Hide();
        }

        public void Show()
        {
            _spriteRenderer.material.SetFloat(_outlinePixelWidth, 1f);
        }
        
        public void Hide()
        {
            _spriteRenderer.material.SetFloat(_outlinePixelWidth, 0f);
        }
    }
}