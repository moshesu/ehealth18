using System;
using Windows.UI.Notifications;
using boost.Core;

namespace boost.PushNotifications
{
	public class MorningNotification : INotificationsFetcher
	{
		private readonly INotificationBuilder _builder;
		private readonly ILocalStorage _localStorage;

		public MorningNotification(INotificationBuilder builder, ILocalStorage localStorage)
		{
			_builder = builder;
			_localStorage = localStorage;
		}

		public ScheduledToastNotification GetNotification(string userId, string name)
		{
			var title = String.Concat("Good Morning ", name);
			var content = "Don't forget to  check out your daily goals!";

			var notificationContent = _builder.Build(title, content);

			var notification = new ScheduledToastNotification(notificationContent.GetXml(), DateTime.Today.AddHours(NotificationTimes.NextMorning));
			notification.Tag = "Morning";

			return notification;
		}
	}
}
