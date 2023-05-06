using System;
using Cysharp.Threading.Tasks;
using EntitySystem.Abstraction;
using Mono.CSharp;
using Player.Interfaces;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerDashHandler: MonoBehaviour, IDamageLock
    {
        [SerializeField]
        private float _dashCost = 10f;
        
        [SerializeField]
        private float _dashForce = 100f;
        public bool IsLocked { get; private set; }

        private IPlayerStamina _playerStamina;
        
        private Rigidbody2D _rigidbody2D;
        
        private Vector2 _move;
        
        private IPlayerStamina PlayerStamina =>
            _playerStamina ??= ServiceLocator.ServiceLocator.Instance.Get<IPlayerStamina>();
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public async void TryDash(InputAction.CallbackContext context)
        {
            if(!context.performed)
                return;
            
            if(!PlayerStamina.HasEnoughStamina(_dashCost))
                return;
            
            if(IsLocked)
                return;
            
            IsLocked = true;
            _rigidbody2D.AddForce(_move * _dashForce, ForceMode2D.Impulse);
            PlayerStamina.DrainStamina(_dashCost);
            await UniTask.Delay(TimeSpan.FromSeconds(.15f));
            
            IsLocked = false;

            Debug.Log("Dash performed");
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>().normalized;
        }
    }
}