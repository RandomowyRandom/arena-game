using System;
using Notifications.Abstraction;
using UnityEngine;

namespace Notifications
{
    public class PlayerNotificationHandler: MonoBehaviour, IPlayerNotificationHandler
    {
        [SerializeField]
        private Notification _notificationPrefab;

        private float _notificationDelay = 1f;
        
        private float _notificationTimer = 0f;

        private void Awake()
        {
            ServiceLocator.ServiceLocator.Instance.Register<IPlayerNotificationHandler>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.ServiceLocator.Instance.Deregister<IPlayerNotificationHandler>();
        }

        private void Update()
        {
            _notificationTimer -= Time.deltaTime;
        }

        public void TrySendNotification(string notificationText)
        {
            if (_notificationTimer > 0f)
                return;
            
            var notification = Instantiate(_notificationPrefab, transform.position, Quaternion.identity);
            notification.SetNotificationText(notificationText);
            
            _notificationTimer = _notificationDelay;
        }
    }
}