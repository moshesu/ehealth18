using boost.Cloud.AzureDatabase;
using boost.Core.App;
using boost.Core.Entities.Users;
using boost.Screens;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Game_Hub : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public static double width;
		public readonly ISignInFlow _signInFlow;
		public readonly IBalanceHandler _IBalanceHandler;
		public readonly UserTypeRepository _IUserTypeRepository;
		Player player;

		public static bool CoachAndPlayer = false;
		public readonly static int TicTacToeGameCost = 15;
		public readonly static int MineSweeperGameCost = 15;

        public Game_Hub()
        {
            this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IBalanceHandler = ProgramContainer.container.GetInstance<IBalanceHandler>();
			_IUserTypeRepository = ProgramContainer.container.GetInstance<UserTypeRepository>();
			player = _signInFlow.GetPlayer();
			CoachAndPlayer = false;
			if (_IUserTypeRepository.GetUserType(player.UserId).UserType == UserType.CoachAndPlayer)
			{
				ToBeACoach.Content = "Switch to Coach mode";
			}
			welcome.Text = "Hey " + player.FirstName + ", welcome to the game hub\n";
            crystals.Text = "You have " + _IBalanceHandler.GetBalance() + " crystals!\n";
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
			_signInFlow.SignOut();
            this.Frame.Navigate(typeof(MainPage));
        }
        private void View_Daily_Click(object sender, RoutedEventArgs e)
        {
			width = outer.ActualWidth;
			this.Frame.Navigate(typeof(PlayerViewDailyGoals));
		}

        private void View_Weekly_Click(object sender, RoutedEventArgs e)
        {
			width = outer.ActualWidth;
            this.Frame.Navigate(typeof(PlayerViewWeeklyGoals));
        }

        private void View_Recent_Activities(object sender , RoutedEventArgs e)
        {
			this.Frame.Navigate(typeof(PlayerViewRecentActivities));
        }

        private void View_Sleep_Summaries(object sender, RoutedEventArgs e)
        {
			this.Frame.Navigate(typeof(PlayerViewSleep));
        }
		private void CompareClick(object sender, RoutedEventArgs e)
		{
			width = outer.ActualWidth;
			this.Frame.Navigate(typeof(PlayerCompareProgress));
		}
		private async void beACoach(object sender , RoutedEventArgs e)
		{
			if (_IUserTypeRepository.GetUserType(player.UserId).UserType == UserType.CoachAndPlayer)
			{
				((SignInFlow)_signInFlow).SaveUserToLocalStorage(UserType.Coach);
				Coach coach = _signInFlow.GetCoach();
				if (coach.FirstName == null) //The coach doesn't have a health account - should not happen.
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
			}
			else
			{
				var messageDialog = new MessageDialog("NOTICE! In order to be a coach you will need to enter a payment method and have a player. Are you sure?");
				messageDialog.Commands.Add(new UICommand("Yes") { Id = 0 });
				messageDialog.Commands.Add(new UICommand("No") { Id = 1 });
				var result = await messageDialog.ShowAsync();
				if ((int)result.Id == 1)
					return;
				CoachAndPlayer = true;
				this.Frame.Navigate(typeof(New_Coach_Explenations));
			}
		}

		private async void TicTacToeClick(object sender, RoutedEventArgs e)
		{
			var messageDialog = new MessageDialog($"NOTICE! A game of TicTacToe costs {Game_Hub.TicTacToeGameCost} Crystals. Are you sure?");
			messageDialog.Commands.Add(new UICommand("Yes") { Id = 0 });
			messageDialog.Commands.Add(new UICommand("No")  { Id = 1 });
			var result = await messageDialog.ShowAsync();
			if ((int)result.Id == 1)
				return;
			if (_IBalanceHandler.GetBalance() < Game_Hub.TicTacToeGameCost)
			{
				var messageDialog2 = new MessageDialog("ERROR: you don't have enough crystals!");
				messageDialog2.Commands.Add(new UICommand("OK") { Id = 0 });
				await messageDialog2.ShowAsync();
				return;
			}
			_IBalanceHandler.SendTransaction(-Game_Hub.TicTacToeGameCost, null);
			this.Frame.Navigate(typeof(GameTicTacToe));
		}
		private void MineSweeperClick(object sender, RoutedEventArgs e)
		{
			
			this.Frame.Navigate(typeof(GameMineSweeperStart));
		}
	}
}
