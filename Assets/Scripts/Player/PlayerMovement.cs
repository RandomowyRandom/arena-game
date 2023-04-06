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
        private Rigidbody2D _rigidbody2D;

        private IStatsDataProvider _playerStats;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _playerStats = GetComponent<IStatsDataProvider>();
        }
        
        private void FixedUpdate()
        {
            const int dragCompensation = 20;

            _rigidbody2D.AddForce(_move * (dragCompensation * _playerStats.GetStatsData().Speed), ForceMode2D.Force);
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }
    }
}