using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Notifications
{
    public class Notification: MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _notificationText;

        private void Start()
        {
            transform.DOMoveY(transform.position.y + 2f, 1f).OnComplete(() =>
            {
                _notificationText.DOFade(0, .2f).OnComplete(() => Destroy(gameObject));
            });
        }

        public void SetNotificationText(string text)
        {
            _notificationText.text = text;
        }
    }
}