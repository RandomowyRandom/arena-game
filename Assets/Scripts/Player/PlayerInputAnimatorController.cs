using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerInputAnimatorController: MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        
        private Camera _camera;
        
        private Vector2 _move;
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _camera = Camera.main;
        }

        private void Update()
        {
            var move = _move.normalized;
            var isMoving = move != Vector2.zero;
            
            var cursorDirection = GetCursorDirection();
            _spriteRenderer.flipX = cursorDirection == 1;

            if (!isMoving)
            {
                _animator.Play("Idle");
                return;
            }

            _animator.Play("Walk");
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }
        
        private int GetCursorDirection()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            var screenPosition = _camera.WorldToScreenPoint(transform.position);
            var direction = mousePosition.x > screenPosition.x ? 1 : -1;
            return direction;
        }
    }
}