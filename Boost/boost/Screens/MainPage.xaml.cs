using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using boost.Cloud.HealthCloud.Authentication;
using boost.Core.App;
using boost.Core.Entities.Users;
using System;
using Windows.UI.Popups;
using Windows.UI;
using Windows.UI.Xaml.Media;
using boost.Cloud.HealthCloud.Exceptions;

namespace boost
{
	public sealed partial class MainPage : Page
	{
		public SolidColorBrush BackgroundColorBrush	= MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush		= MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();
		public SolidColorBrush ProgressBackgroundColorBrush = MainPage.getProgressBackgroundColorBrush();
		public SolidColorBrush ProgressForegroundColorBrush = MainPage.getProgressForegroundColorBrush();
		public SolidColorBrush CoachRewardActivityGridBackgroundColorBrush = MainPage.getCoachRewardActivityGridBackgroundColorBrush();
		public SolidColorBrush ProgressCompareOthersColorBrush = MainPage.getProgressCompareOthersColorBrush();
		public SolidColorBrush ProgressComparePlayerColorBrush = MainPage.getProgressComparePlayerColorBrush();
		public SolidColorBrush ProgressRectFillColorBrush = MainPage.getProgressRectFillColorBrush();

		public static SolidColorBrush getBackgroundColorBrush()
		{
			return new SolidColorBrush(Colors.SkyBlue);
		}
		public static SolidColorBrush getTextOnBackgroundColorBrush()
		{
			return  new SolidColorBrush(Colors.Black);
		}
		public static SolidColorBrush getButtonColorBrush()
		{
			return new SolidColorBrush(Colors.Azure); 
		}
		public static SolidColorBrush getButtonTextColorBrush()
		{
			return new SolidColorBrush(Colors.Black);
		}
		public static SolidColorBrush getLogOutButtonColorBrush()
		{
			return new SolidColorBrush(Colors.IndianRed);
		}
		public static SolidColorBrush getProgressBackgroundColorBrush()
		{
			return new SolidColorBrush(Colors.White);
		}
		public static SolidColorBrush getProgressForegroundColorBrush()
		{
			return new SolidColorBrush(Colors.YellowGreen);
		}
		public static SolidColorBrush getCoachRewardActivityGridBackgroundColorBrush()
		{
			return new SolidColorBrush(Colors.DimGray);
		}
		public static SolidColorBrush getProgressCompareOthersColorBrush()
		{
			return new SolidColorBrush(Colors.SteelBlue);
		}
		public static SolidColorBrush getProgressComparePlayerColorBrush()
		{
			return new SolidColorBrush(Colors.IndianRed);
		}
		public static SolidColorBrush getProgressRectFillColorBrush()
		{
			return new SolidColorBrush(Colors.Black);
		}
		

		public readonly ISignInFlow _signInFlow;

		public MainPage()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
		}

		public async void log_in_button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var _tokenGenerator = new HealthTokenGenerator();
				await _tokenGenerator.GetHealthAuthenticationToken();
				var userType = _signInFlow.SignIn();

				if (userType == UserType.CoachAndPlayer)
				{
					var messageDialog = new MessageDialog("Hey! We noticed you are both a coach and a player. As what would you like to log in as?");
					messageDialog.Commands.Add(new UICommand("Coach") { Id = 0 });
					messageDialog.Commands.Add(new UICommand("Player") { Id = 1 });

					var result = await messageDialog.ShowAsync();
					if ((int)result.Id == 0)
						userType = UserType.Coach;
					else
						userType = UserType.Player;
					((SignInFlow)_signInFlow).SaveUserToLocalStorage(userType);
				}
				switch (userType)
				{
					case UserType.None:

						this.Frame.Navigate(typeof(New_User));
						break;
					case UserType.Player:

						Player player = _signInFlow.GetPlayer();
						if (player.FirstName == null) //The coach doesn't have a health account
						{
							var messageDialog = new MessageDialog("Hi new player, we noticed you don't have a microsoft health account. Please go to microsoft's health app and create one.");
							messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
							var result = await messageDialog.ShowAsync();
							_signInFlow.SignOut();
							return;
						}
						this.Frame.Navigate(typeof(Game_Hub));
						break;
					case UserType.Coach:

						Coach coach = _signInFlow.GetCoach();
						if(coach.FirstName == null) //The coach doesn't have a health account
						{
							var messageDialog = new MessageDialog("Hi new coach, we noticed you don't have a microsoft health account. Please go to microsoft's health app and create one.");
							messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
							var result = await messageDialog.ShowAsync();
							_signInFlow.SignOut();
							return;
						}
				
						else if (coach.PaymentLastDigits == "") //the coach hasn't completed his sign up
						{
							var messageDialog = new MessageDialog("Hey! We noticed you haven't completed your Sign In, so we will now continue!");
							messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
							var result = await messageDialog.ShowAsync();
							this.Frame.Navigate(typeof(New_Coach_CreditCard));
						}
						else //the coach is OK.
							this.Frame.Navigate(typeof(Coach_Lobby));
						break;
					case UserType.UnassignedPlayer:

						this.Frame.Navigate(typeof(New_Player));
						break;

				}
			}
			catch (UserDeniedAccessExeption)
			{
				var messageDialog = new MessageDialog("An error occured:  User denied access. Please try again.");
				messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				_signInFlow.SignOut();
				return;
			}
			catch (UserCancelSignInExeption)
			{
				var messageDialog = new MessageDialog("An error occured:  User canceled Sign In. Please try again.");
				messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				_signInFlow.SignOut();
				return;
			}
			catch (TokenAcquiringException)
			{
				var messageDialog = new MessageDialog("An error occured:  Token acquiring failed. Please try again.");
				messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				_signInFlow.SignOut();
				return;
			}
			catch (Exception exc)
			{
				var messageDialog = new MessageDialog("An unknown error occured. Please try again.");
				messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				_signInFlow.SignOut();
				return;
			}





		}

		private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(New_User));
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(NewUnassignedPlayer));
		}
	}
}
