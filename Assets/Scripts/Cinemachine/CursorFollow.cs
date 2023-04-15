using System;
using UnityEngine;

namespace Cinemachine
{
    public class CursorFollow: MonoBehaviour
    {
        private bool _isRectTransform;
        private Camera _camera;
        
        private void Awake()
        {
            _isRectTransform = GetComponent<RectTransform>() != null;
        }
        
        private void Start()
        {
            _camera = Camera.main;
        }
        
        private void Update()
        {
            if (_isRectTransform)
            {
                var canvas = GetComponentInParent<Canvas>();
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out var rectPosition);
                transform.position = canvas.transform.TransformPoint(rectPosition);
            }
            
            var position = _camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(position.x, position.y, 0);
        }
    }
}