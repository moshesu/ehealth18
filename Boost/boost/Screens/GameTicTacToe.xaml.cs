using boost.Core.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
	public sealed partial class GameTicTacToe : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		char[,] clicks = new char[3,3] ;
		char player = 'X';
		bool end = false;
		Button[,] butts = new Button[3,3];

		public readonly IBalanceHandler _IBalanceHandler;
		public GameTicTacToe()
		{
			this.InitializeComponent();
			_IBalanceHandler = ProgramContainer.container.GetInstance<IBalanceHandler>();
			Init();
		}
		private void Init()
		{
			end = false;
			Winner.Text = "";
			player = 'X';
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (inner.Children.Contains(butts[i, j]))
						inner.Children.Remove(butts[i, j]);
					clicks[i, j] = '0';
					butts[i,j] = new Button()
					{
						Content = "",
						Name = "" + i + "," + j,
						Background = new SolidColorBrush(Colors.Aquamarine),
						FontSize = 70,
						Foreground = new SolidColorBrush(Colors.Red),
					};
					butts[i, j].SetValue(Grid.RowProperty, i);
					butts[i, j].SetValue(Grid.ColumnProperty, j);
					butts[i, j].SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
					butts[i, j].SetValue(VerticalAlignmentProperty, VerticalAlignment.Stretch);
					butts[i, j].BorderThickness = new Thickness(2, 2, 2, 2);
					butts[i, j].BorderBrush = new SolidColorBrush(Colors.DarkGray);
					butts[i, j].Click += new RoutedEventHandler(HandleClick);
					inner.Children.Add(butts[i, j]);

				}
			}
		}
		private void HandleClick(object sender, RoutedEventArgs e)
		{
			if (end)
				return;
			int i = ((Button)sender).Name[0] - '0';
			int j = ((Button)sender).Name[2] - '0';
			if (clicks[i, j] != '0')
				return;
			clicks[i, j] = player;
			butts[i, j].Content = player;
			char x = CheckWin();
			if (x != '0')
			{
				Winner.Text = "" + x + " Player won!";
				end = true;
				return;
			}
			player = (char)('X' + 'O' - player);
			for (int a = 0; a < 3; a++)
			{
				for (int b = 0; b < 3; b++)
				{
					if (clicks[a, b] == '0')
						return;
				}
			}
			Winner.Text = "It's a Tie!";
			end = true;
					
		}
		private char CheckWin()
		{
			for (int i = 0; i < 3; i++)
			{
				if (clicks[i, 0] == clicks[i, 1] && clicks[i, 1] == clicks[i, 2] && clicks[i, 2] != '0')
				{
					butts[i, 0].Background = new SolidColorBrush(Colors.Green);
					butts[i, 1].Background = new SolidColorBrush(Colors.Green);
					butts[i, 2].Background = new SolidColorBrush(Colors.Green);
					return clicks[i, 0];
				}
			}
			for (int j = 0; j < 3; j++)
			{
				if (clicks[0, j] == clicks[1, j] && clicks[1, j] == clicks[2, j] && clicks[2, j] != '0')
				{
					butts[0, j].Background = new SolidColorBrush(Colors.Green);
					butts[1, j].Background = new SolidColorBrush(Colors.Green);
					butts[2, j].Background = new SolidColorBrush(Colors.Green);
					return clicks[0, j];
				}
			}
			if (clicks[0, 0] == clicks[1, 1] && clicks[1, 1] == clicks[2, 2] && clicks[2, 2] != '0')
			{
				butts[0, 0].Background = new SolidColorBrush(Colors.Green);
				butts[1, 1].Background = new SolidColorBrush(Colors.Green);
				butts[2, 2].Background = new SolidColorBrush(Colors.Green);
				return clicks[1, 1];
			}
			if (clicks[2, 0] == clicks[1, 1] && clicks[1, 1] == clicks[0, 2] && clicks[0, 2] != '0')
			{
				butts[0, 2].Background = new SolidColorBrush(Colors.Green);
				butts[1, 1].Background = new SolidColorBrush(Colors.Green);
				butts[2, 0].Background = new SolidColorBrush(Colors.Green);
				return clicks[1, 1];
			}
			return '0';
		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Game_Hub));
		}
		private async void Reset(object sender, RoutedEventArgs e)
		{
			var messageDialog = new MessageDialog($"NOTICE! A game of TicTacToe costs {Game_Hub.TicTacToeGameCost} Crystals. Are you sure?");
			messageDialog.Commands.Add(new UICommand("Yes") { Id = 0 });
			messageDialog.Commands.Add(new UICommand("No") { Id = 1 });
			var result = await messageDialog.ShowAsync();
			if ((int)result.Id == 1)
				return;
			if (_IBalanceHandler.GetBalance() < Game_Hub.TicTacToeGameCost )
			{
				var messageDialog3 = new MessageDialog("ERROR: you don't have enough crystals!");
				messageDialog3.Commands.Add(new UICommand("OK") { Id = 0 });
				await messageDialog3.ShowAsync();
				return;
			}
			_IBalanceHandler.SendTransaction(-Game_Hub.TicTacToeGameCost, null);
			Init();
		}
	}
}
