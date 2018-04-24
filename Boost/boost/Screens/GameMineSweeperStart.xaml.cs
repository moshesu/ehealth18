using boost.Core.App;
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
	public sealed partial class GameMineSweeperStart : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public static double width;
		public static double height;
		public readonly IBalanceHandler _IBalanceHandler;
		public GameMineSweeperStart()
		{
			this.InitializeComponent();
			_IBalanceHandler = ProgramContainer.container.GetInstance<IBalanceHandler>();
		}

		private async void EasyButton_Click(object sender, RoutedEventArgs e)
		{
			width  = outer.ActualWidth;
			height = outer.ActualHeight;
			var messageDialog2 = new MessageDialog($"NOTICE! A game of Mine Sweeper costs {Game_Hub.MineSweeperGameCost} Crystals. Are you sure?");
			messageDialog2.Commands.Add(new UICommand("Yes") { Id = 0 });
			messageDialog2.Commands.Add(new UICommand("No") { Id = 1 });
			var result2 = await messageDialog2.ShowAsync();
			if ((int)result2.Id == 1)
			{
				return;
			}
			if (_IBalanceHandler.GetBalance() < Game_Hub.MineSweeperGameCost)
			{
				var messageDialog3 = new MessageDialog("ERROR: you don't have enough crystals!");
				messageDialog3.Commands.Add(new UICommand("OK") { Id = 0 });
				await messageDialog3.ShowAsync();
				return;
			}
			_IBalanceHandler.SendTransaction(-Game_Hub.MineSweeperGameCost, null);
			this.Frame.Navigate(typeof(GameMineSweeperEasy));
		}

		private async void MediumButton_Click(object sender, RoutedEventArgs e)
		{
			width = outer.ActualWidth;
			height = outer.ActualHeight;
			var messageDialog2 = new MessageDialog($"NOTICE! A game of Mine Sweeper costs {Game_Hub.MineSweeperGameCost} Crystals. Are you sure?");
			messageDialog2.Commands.Add(new UICommand("Yes") { Id = 0 });
			messageDialog2.Commands.Add(new UICommand("No") { Id = 1 });
			var result2 = await messageDialog2.ShowAsync();
			if ((int)result2.Id == 1)
			{
				return;
			}
			if (_IBalanceHandler.GetBalance() < Game_Hub.MineSweeperGameCost)
			{
				var messageDialog3 = new MessageDialog("ERROR: you don't have enough crystals!");
				messageDialog3.Commands.Add(new UICommand("OK") { Id = 0 });
				await messageDialog3.ShowAsync();
				return;
			}
			_IBalanceHandler.SendTransaction(-Game_Hub.MineSweeperGameCost, null);
			this.Frame.Navigate(typeof(GameMineSweeperMedium));
		}

		private async void HardButton_Click(object sender, RoutedEventArgs e)
		{
			width = outer.ActualWidth;
			height = outer.ActualHeight;
			var messageDialog2 = new MessageDialog($"NOTICE! A game of Mine Sweeper costs {Game_Hub.MineSweeperGameCost} Crystals. Are you sure?");
			messageDialog2.Commands.Add(new UICommand("Yes") { Id = 0 });
			messageDialog2.Commands.Add(new UICommand("No") { Id = 1 });
			var result2 = await messageDialog2.ShowAsync();
			if ((int)result2.Id == 1)
			{
				return;
			}
			if (_IBalanceHandler.GetBalance() < Game_Hub.MineSweeperGameCost)
			{
				var messageDialog3 = new MessageDialog("ERROR: you don't have enough crystals!");
				messageDialog3.Commands.Add(new UICommand("OK") { Id = 0 });
				await messageDialog3.ShowAsync();
				return;
			}
			_IBalanceHandler.SendTransaction(-Game_Hub.MineSweeperGameCost, null);
			this.Frame.Navigate(typeof(GameMineSweeperHard));
		}
		private void Back_Click(object sender , RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Game_Hub));
		}
	}
}
