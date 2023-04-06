using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement: MonoBehaviour
    {
        [SerializeField]
        private float _speed = 5f;
        
        private Vector2 _move;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            _rigidbody2D.AddForce(_move * (20 * _speed), ForceMode2D.Force);
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }
    }
}