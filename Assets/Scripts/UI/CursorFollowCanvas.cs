using UnityEngine;

namespace UI
{
    public class CursorFollowCanvas: MonoBehaviour
    {
        [SerializeField]
        private Canvas _canvas;
        
        private void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform, 
                Input.mousePosition, 
                _canvas.worldCamera, 
                out var rectPosition);
            
            transform.position = _canvas.transform.TransformPoint(rectPosition);
        }
    }
}