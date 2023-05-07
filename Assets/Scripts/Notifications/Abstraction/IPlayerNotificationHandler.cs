using ServiceLocator;

namespace Notifications.Abstraction
{
    public interface IPlayerNotificationHandler: IService
    {
        public void TrySendNotification(string notificationText, float shownTime = 1f);
        public void ForceSendNotification(string notificationText, float shownTime = 1f);
    }
}