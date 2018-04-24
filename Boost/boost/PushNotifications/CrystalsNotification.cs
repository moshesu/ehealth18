using System;
using Windows.UI.Notifications;
using boost.Core.App;

namespace boost.PushNotifications
{
	public class CrystalsNotification : INotificationsFetcher
	{
		private readonly INotificationBuilder _builder;
		private readonly IBalanceHandler _balanceHandler;

		public CrystalsNotification(
			INotificationBuilder builder,
			IBalanceHandler balanceHandler)
		{
			_builder = builder;
			_balanceHandler = balanceHandler;
		}

		public ScheduledToastNotification GetNotification(string userId, string name)
		{
			var balance = _balanceHandler.GetBalance(userId);

			if (balance <= 0)
				return null;

			var title = String.Concat(name, " dont't Forget!");
			var content = "You still have crystals waiting for you!";

			var notificationContent = _builder.Build(title, content);

			var notification = new ScheduledToastNotification(notificationContent.GetXml(), NotificationTimes.GetTimeToNotify(NotificationTimes.CrystalsHour));
			notification.Tag = "Crystals";

			return notification;
		}
	}
}
