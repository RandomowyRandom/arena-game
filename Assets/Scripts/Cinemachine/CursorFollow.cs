using System;
using UnityEngine;

namespace Cinemachine
{
    public class CursorFollow: MonoBehaviour
    {
        private void Update()
        {
            var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(position.x, position.y, 0);
        }
    }
}