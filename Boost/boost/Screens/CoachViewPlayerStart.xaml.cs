using boost.Core.App;
using boost.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost.Screens
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CoachViewPlayerStart : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public static double width;
		public readonly ISignInFlow _signInFlow;
		public readonly IBalanceHandler _IBalanceHandler;
		public readonly IGoalsHandler _IGoalsHandler;
		public readonly ICoachPlayersPage _ICoachPlayersPage;

		

		public CoachViewPlayerStart()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IBalanceHandler = ProgramContainer.container.GetInstance<IBalanceHandler>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			Player player = (_ICoachPlayersPage.GetPlayers())[Coach_View_Players.Tag];

			string FullName = player.FirstName + player.LastName;

			welcome.Text = $"Viewing {FullName}'s Activities and Summaries\n";
			crystals.Text = $"{FullName} has {_IBalanceHandler.GetBalance(player.UserId)} crystals!\n";
		}

		private void LogOutClick(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Coach_View_Players));
		}
		private void View_Daily_Click(object sender, RoutedEventArgs e)
		{
			width = outer.ActualWidth;
			this.Frame.Navigate(typeof(CoachViewPlayerGoals));
		}

		private void View_Weekly_Click(object sender, RoutedEventArgs e)
		{
			width = outer.ActualWidth;
			this.Frame.Navigate(typeof(CoachViewPlayerWeeklyGoals));
		}

		private void View_Recent_Activities(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerRecentActivities));
		}

		private void View_Sleep_Summaries(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerSleep));
		}
		private void Compare_Progress(object sender, RoutedEventArgs e)
		{
			width = outer.ActualWidth;
			this.Frame.Navigate(typeof(CoachViewPlayerComparison));
		}

		private async void TicTacToeClick(object sender, RoutedEventArgs e)
		{
			var messageDialog = new MessageDialog($"A game of TicTacToe costs {Game_Hub.TicTacToeGameCost} Crystals.");
			messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
			await messageDialog.ShowAsync();
			return;
		}

		private async void MineSweeperClick(object sender, RoutedEventArgs e)
		{
			var messageDialog = new MessageDialog($"A game of MineSweeper costs {Game_Hub.MineSweeperGameCost} Crystals.");
			messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
			await messageDialog.ShowAsync();
			return;
		}
	}
}
