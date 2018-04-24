using Microsoft.Toolkit.Uwp.Notifications;

namespace boost.PushNotifications
{
	public interface INotificationBuilder
	{
		ToastContent Build(string title, string content);
	}

	public class NotificationBuilder : INotificationBuilder
	{
		private readonly string _logo;
		private readonly string _sportsPicture;

		public NotificationBuilder()
		{
			_logo = "../PushNotifications/logoBlue.png";
			_sportsPicture = "../PushNotifications/sports.png";
		}

		public ToastContent Build(string title, string content)
		{
			return new ToastContent (){
				Visual = new ToastVisual()
				{
					BindingGeneric = new ToastBindingGeneric()
					{
						Children =
						{
							new AdaptiveText()
							{
								Text = title
							},

							new AdaptiveText()
							{
								Text = content
							},

							new AdaptiveImage()
							{
								Source = _sportsPicture
							}
						},
						AppLogoOverride = new ToastGenericAppLogo()
						{
							Source = _logo,
							HintCrop = ToastGenericAppLogoCrop.Circle,
						}
					}
				}
			};
		}
	}
}
