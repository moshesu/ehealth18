using System.Collections.Generic;
using System.Linq;
using Windows.UI.Notifications;
using boost.Core;
using boost.Util;

namespace boost.PushNotifications
{
	public interface INotificationsCenter
	{
		void Initialize();
		void Update();
	}

	public class NotificationsCenter : INotificationsCenter
	{
		private readonly INotificationBuilder _builder;
		private readonly IEnumerable<INotificationsFetcher> _fetchers;
		private readonly ILocalStorage _localStorage;
		private string _name;
		private string _userId;

		public NotificationsCenter(INotificationBuilder builder, ILocalStorage localStorage, IEnumerable<INotificationsFetcher> fetchers)
		{
			_builder = builder;
			_localStorage = localStorage;
			_fetchers = fetchers;
		}

		public void Initialize()
		{
			_name = _localStorage.GetProfileInfo().FirstName;
			_userId = _localStorage.GetCurrentUserId();
			ToastNotificationManager.History.Clear();

			foreach (var fetcher in _fetchers)
			{
				var notification = fetcher.GetNotification(_userId, _name);

				if(notification!=null)
					ToastNotificationManager.CreateToastNotifier().AddToSchedule(notification);
			}
		}

		public void Update()
		{
			IReadOnlyList<ScheduledToastNotification> scheduled =
				ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();

			foreach (var fetcher in _fetchers)
			{
				var notification = fetcher.GetNotification(_userId, _name);

				if (notification != null)
					{
					scheduled.Where(x => x.Tag == notification.Tag)
							.ForEach(x => ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(x));

						ToastNotificationManager.CreateToastNotifier().AddToSchedule(notification);
				}
			}
		}
	}
}
