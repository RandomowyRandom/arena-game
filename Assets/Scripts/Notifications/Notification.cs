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

        public void SetNotificationText(string text, float shownTime = 1f)
        {
            _notificationText.text = text;
            
            transform.DOMoveY(transform.position.y + 2f, shownTime).OnComplete(() =>
            {
                _notificationText.DOFade(0, .2f).OnComplete(() => Destroy(gameObject));
            });
        }
    }
}