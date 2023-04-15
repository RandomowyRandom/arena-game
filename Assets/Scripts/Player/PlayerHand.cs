using System;
using Items;
using Items.ItemDataSystem;
using Player.Interfaces;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerHand: SerializedMonoBehaviour
    {
        [SerializeField]
        private Transform _center;
        
        [SerializeField]
        private SpriteRenderer _handRenderer;
        
        [SerializeField]
        private float _maxDistanceFromCenter;
        
        [OdinSerialize]
        private IUsableItemProvider _usableItemProvider;
        
        private Camera _camera;

        public void SetItem(ItemData item)
        {
            if (item == null)
            {
                _handRenderer.sprite = null;
                return;                
            }
            
            _handRenderer.sprite = item.Icon;
        }

        private void RefreshItem()
        {
            SetItem(_usableItemProvider.GetUsableItem());
        }
        
        private void Start()
        {
            _camera = Camera.main;
            
            _usableItemProvider.OnUsableItemChanged += RefreshItem;
            
            var usableItem = _usableItemProvider.GetUsableItem();
            
            if(usableItem != null)
                SetItem(usableItem);
        }
        
        private void OnDestroy()
        {
            _usableItemProvider.OnUsableItemChanged -= RefreshItem;
        }

        private void Update()
        {
            HandleHandMovement();
        }

        private void HandleHandMovement()
        {
            var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var direction = mousePosition - _center.position;
            var distance = Mathf.Clamp(direction.magnitude, 0, _maxDistanceFromCenter);
            var targetPosition = _center.position + direction.normalized * distance;
            transform.position = targetPosition;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            _handRenderer.flipY = angle is > 90 or < -90;
        }
    }
}