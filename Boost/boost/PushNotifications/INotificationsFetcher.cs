using Windows.UI.Notifications;
namespace boost.PushNotifications
{
	public interface INotificationsFetcher
	{
		ScheduledToastNotification GetNotification(string userId, string name);
	}
}
