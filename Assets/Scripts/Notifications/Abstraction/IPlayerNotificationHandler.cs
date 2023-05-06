using ServiceLocator;

namespace Notifications.Abstraction
{
    public interface IPlayerNotificationHandler: IService
    {
        public void TrySendNotification(string notificationText);
    }
}