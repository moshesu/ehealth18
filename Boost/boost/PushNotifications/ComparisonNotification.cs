using System;
using Windows.UI.Notifications;
using boost.Core;
using boost.Core.App;

namespace boost.PushNotifications
{
	public class ComparisonNotification : INotificationsFetcher
	{
		private readonly INotificationBuilder _builder;
		private readonly ICompareUser _comparisonFetcher;
		//		private readonly ILocalStorage _localStorage;

		public ComparisonNotification(INotificationBuilder builder, ILocalStorage localStorage, ICompareUser comparisonFetcher)
		{
			_builder = builder;
			_comparisonFetcher = comparisonFetcher;
		}

		public ScheduledToastNotification GetNotification(string userId, string name)
		{
			var comparisonMessage = GetComparisonMessage(userId);

			if (comparisonMessage == null)
				return null;

			var title = String.Concat("Good work ", name, "!");
			var content = String.Concat("You ", comparisonMessage, " of players your age. That's awesome! Keep it up!");

			var notificationContent = _builder.Build(title, content);

			var notification = new ScheduledToastNotification(notificationContent.GetXml(), DateTime.Now.AddMinutes(NotificationTimes.ComparisonMinutes));
			notification.Tag = "Comparison";

			return notification;
		}

		private string GetComparisonMessage(string userId)
		{
			var comparison = _comparisonFetcher.GetComparison(userId);
			var userStepsPercentile = comparison.StepsComparison.UserPercentile;
			var userActivityPercentile = comparison.ActiveHoursComparison.UserPercentile;

			if (userActivityPercentile > userStepsPercentile && userActivityPercentile > 5)
			{
				var percentage = String.Concat(userActivityPercentile.ToString(), "0%");
				return String.Concat("are more active than ", percentage);
			}

			if (userStepsPercentile > 5)
			{
				var percentage = String.Concat(userStepsPercentile.ToString(), "0%");
				return String.Concat("take more steps than ", percentage);
			}

			return null;
		}
	}
}
