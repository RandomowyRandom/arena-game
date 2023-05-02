using System;
using Player.Interfaces;
using QFSW.QC;
using Stats.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(IStatsDataProvider))]
    public class PlayerMovement: MonoBehaviour
    {
        private Vector2 _move;
        private bool _isSprinting;
        private Rigidbody2D _rigidbody2D;

        private IPlayerStats _playerStats;
        private IPlayerStamina _playerStamina;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _playerStats = ServiceLocator.ServiceLocator.Instance.Get<IPlayerStats>();
            _playerStamina = ServiceLocator.ServiceLocator.Instance.Get<IPlayerStamina>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.AddForce(GetMovementVector(), ForceMode2D.Force);
        }
        
        private Vector2 GetMovementVector()
        {
            const float sprintCost = .8f;
            const float sprintMultiplier = 1.5f;
            const int dragCompensation = 20;

            var movementVector = _move * (_playerStats.GetStatsData().Speed * dragCompensation);
            
            if (!_isSprinting || !_playerStamina.HasEnoughStamina(sprintCost)) 
                return movementVector;
            
            movementVector *= sprintMultiplier;
            _playerStamina.DrainStamina(sprintCost);

            return movementVector;
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            _isSprinting = context.action.IsPressed();
        }
    }
}