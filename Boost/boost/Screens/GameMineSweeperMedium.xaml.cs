﻿using boost.Core.App;
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
	public sealed partial class GameMineSweeperMedium : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		static int dimensionX = 16;
		static int dimensionY = 16;

		static int parameterX = 448;
		static int parameterY = 64;

		static int MAXbombcount = 32;
		static int bombcount = MAXbombcount;
		static int flagcount = 40;

		static double SIZE = Math.Min(GameMineSweeperStart.width, GameMineSweeperStart.height) * 0.95 / dimensionX;
		static bool Flags = false;
		static bool Disarm = true;
		Button[,] buttons = new Button[dimensionX, dimensionY];

		int[,] board = new int[dimensionX, dimensionY];
		int[,] oldboard = new int[dimensionX, dimensionY];
		int[,] statusboard = new int[dimensionX, dimensionY];

		public readonly IBalanceHandler _IBalanceHandler;
		public GameMineSweeperMedium()
		{
			this.InitializeComponent();
			_IBalanceHandler = ProgramContainer.container.GetInstance<IBalanceHandler>();

			StackPanel gameboardGrid = new StackPanel();
			gameboardGrid.BorderThickness = new Thickness(1, 1, 1, 1);
			gameboardGrid.BorderBrush = new SolidColorBrush(Colors.Gray);

			addMines(board);
			addMinesIndicators(board);

			addMines(oldboard);
			addMinesIndicators(oldboard);

			initializeStatusBoard(statusboard);

			for (int i = 0; i < dimensionX; i++)
			{
				StackPanel rowStackPanel = new StackPanel();
				rowStackPanel.Orientation = Orientation.Horizontal;
				for (int j = 0; j < dimensionY; j++)
				{
					buttons[i, j] = new Button();

					addMineClick(i, j);

					addNonMineClick(i, j);

					addRightClick(i, j);

					buttons[i, j].Height = SIZE;
					buttons[i, j].Width = SIZE;
					buttons[i, j].BorderThickness = new Thickness(2, 2, 2, 2);
					buttons[i, j].BorderBrush = new SolidColorBrush(Colors.DarkGray);

					rowStackPanel.Children.Add(buttons[i, j]);
				}
				gameboardGrid.Children.Add(rowStackPanel);
			}
			signBoardStackPanel.Children.Clear();
			signBoardStackPanel.Children.Add(gameboardGrid);
		}



		void initializeStatusBoard(int[,] statusboard)
		{
			for (int i = 0; i < dimensionX; i++)
				for (int j = 0; j < dimensionY; j++)
					statusboard[i, j] = 0;
		}


		private void MineClick(object s, RoutedEventArgs e)
		{
			for (int i1 = 0; i1 < dimensionX; i1++)
				for (int j1 = 0; j1 < dimensionY; j1++)
				{
					if (board[i1, j1] == 9)
					{
						buttons[i1, j1].Content = "X";
						buttons[i1, j1].Background = new SolidColorBrush(Colors.Red);
					}
				}
			bombcount = MAXbombcount;
			flagcount = 99;
			LoseButton_Click(s, e);
		}

		void addMineClick(int i, int j)
		{
			if (board[i, j] == 9)
			{
				buttons[i, j].Click += new RoutedEventHandler(MineClick);
			}
		}

		private void NonMineClick(object s, RoutedEventArgs e)
		{
			var button = s as Button;
			var ttv = button.TransformToVisual(Window.Current.Content);
			Point screenCoords = ttv.TransformPoint(new Point(0, 0));

			parameterY = Convert.ToInt32(buttons[0, 0].TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0)).Y);
			parameterX = Convert.ToInt32(buttons[0, 0].TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0)).X);

			int coordY = Convert.ToInt32((screenCoords.Y - parameterY) / SIZE);
			int coordX = Convert.ToInt32((screenCoords.X - parameterX) / SIZE);


			if (board[coordY, coordX] == 0)
				Fill(buttons, coordY, coordX);
			else
			{
				if (statusboard[coordY, coordX] == 2)
				{
					flagcount++;
				}
				statusboard[coordY, coordX] = 1;
				buttons[coordY, coordX].Content = board[coordY, coordX];
				buttons[coordY, coordX].Background = new SolidColorBrush(Colors.Green);
			}
		}
		void addNonMineClick(int i, int j)
		{
			if (board[i, j] != 9)
			{
				buttons[i, j].Click += new RoutedEventHandler(NonMineClick);
			}
		}
		private void RightClick(object s, RoutedEventArgs e)
		{
			var button = s as Button;
			var ttv = button.TransformToVisual(Window.Current.Content);
			Point screenCoords = ttv.TransformPoint(new Point(0, 0));

			parameterY = Convert.ToInt32(buttons[0, 0].TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0)).Y);
			parameterX = Convert.ToInt32(buttons[0, 0].TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0)).X);

			int coordY = Convert.ToInt32((screenCoords.Y - parameterY) / SIZE);
			int coordX = Convert.ToInt32((screenCoords.X - parameterX) / SIZE);
			if (statusboard[coordY, coordX] == 0)
			{
				flagcount--;
				statusboard[coordY, coordX] = 2;
				if (board[coordY, coordX] == 9)
					bombcount--;
				buttons[coordY, coordX].Content = "cat";
				buttons[coordY, coordX].Background = new SolidColorBrush(Colors.Yellow);
			}
			else if (statusboard[coordY, coordX] == 2)
			{
				flagcount++;
				statusboard[coordY, coordX] = 0;
				if (board[coordY, coordX] == 9)
					bombcount++;
				buttons[coordY, coordX].Content = "";
				buttons[coordY, coordX].Background = new SolidColorBrush(Colors.Blue);
			}
			if (bombcount == 0)
			{
				bombcount = MAXbombcount;
				flagcount = 99;
				WinButton(s, e);
			}
		}

		void addRightClick(int i, int j)
		{
			buttons[i, j].RightTapped += new RightTappedEventHandler(RightClick);
		}
		private void FlagsButton_Click(object sender, RoutedEventArgs e)
		{
			if (Flags)
				return;
			Disarm = false;
			Flags = true;
			for (int i = 0; i < dimensionX; i++)
			{
				for (int j = 0; j < dimensionY; j++)
				{
					buttons[i, j].Click -= new RoutedEventHandler(MineClick); // remove cliock handlers
					buttons[i, j].Click -= new RoutedEventHandler(NonMineClick);
					buttons[i, j].Click += new RoutedEventHandler(RightClick); // for each button, treat left click as if it was a right click
				}
			}
		}
		private void DisarmMinesButton_Click(object sender, RoutedEventArgs e)
		{
			if (Disarm)
				return;
			Disarm = true;
			Flags = false;
			for (int i = 0; i < dimensionX; i++)
			{
				for (int j = 0; j < dimensionY; j++)
				{
					buttons[i, j].Click -= new RoutedEventHandler(RightClick); // remove "right click" handler
					addMineClick(i, j); // for each button, treat left click as usual
					addNonMineClick(i, j);
				}
			}
		}


		private async void WinButton(object sender, RoutedEventArgs e)
		{
			var messageDialog = new MessageDialog("ALL YOUR CATS HAVE BEEN SAVED! Pusica will be so happy to play with them :D. But we can still save some kittens ! What do you want to do?");
			messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("Restart") { Id = 0 });
			messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("Main Menu") { Id = 1 });
			var result = await messageDialog.ShowAsync();

			if ((int)result.Id == 0)
			{
				var messageDialog2 = new MessageDialog($"NOTICE! A game of Mine Sweeper costs {Game_Hub.MineSweeperGameCost} Crystals. Are you sure?");
				messageDialog2.Commands.Add(new UICommand("Yes") { Id = 0 });
				messageDialog2.Commands.Add(new UICommand("No") { Id = 1 });
				var result2 = await messageDialog2.ShowAsync();
				if ((int)result2.Id == 1)
				{
					WinButton(sender, e);
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
			else
			{
				this.Frame.Navigate(typeof(GameMineSweeperStart));
			}
		}

		void Fill(Button[,] screen, int x, int y)
		{

			if (x < 0 || x >= dimensionX || y < 0 || y >= dimensionY)
				return;

			if (oldboard[x, y] != 0)
			{
				if (board[x, y] != 0)
					buttons[x, y].Content = board[x, y];

				buttons[x, y].Background = new SolidColorBrush(Colors.Green);

				if (statusboard[x, y] == 2)
				{
					flagcount++;
				}

				return;
			}

			if (statusboard[x, y] == 2)
			{
				flagcount++;
			}

			statusboard[x, y] = 1;
			buttons[x, y].Background = new SolidColorBrush(Colors.Green);
			buttons[x, y].Content = "";
			oldboard[x, y] = 1;

			Fill(screen, x + 1, y);
			Fill(screen, x - 1, y);
			Fill(screen, x, y + 1);
			Fill(screen, x, y - 1);

			Fill(screen, x + 1, y + 1);
			Fill(screen, x - 1, y - 1);
			Fill(screen, x - 1, y + 1);
			Fill(screen, x + 1, y - 1);
		}

		private async void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			bombcount = MAXbombcount;
			flagcount = 99;
			var messageDialog = new MessageDialog("What do you want to do?");
			messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("Restart") { Id = 0 });
			messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("Main Menu") { Id = 1 });
			var result = await messageDialog.ShowAsync();

			if ((int)result.Id == 0)
			{
				var messageDialog2 = new MessageDialog($"NOTICE! A game of Mine Sweeper costs {Game_Hub.MineSweeperGameCost} Crystals. Are you sure?");
				messageDialog2.Commands.Add(new UICommand("Yes") { Id = 0 });
				messageDialog2.Commands.Add(new UICommand("No") { Id = 1 });
				var result2 = await messageDialog2.ShowAsync();
				if ((int)result2.Id == 1)
					return;
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
			else
			{
				this.Frame.Navigate(typeof(GameMineSweeperStart));
			}
		}

		private async void LoseButton_Click(object sender, RoutedEventArgs e)
		{
			var messageDialog = new MessageDialog("ALL YOUR CATS HAVE BLOWN! Pusica will be so sad :(. But we can still save some kittens ! What do you want to do?");
			messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("Restart") { Id = 0 });
			messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("Main Menu") { Id = 1 });
			var result = await messageDialog.ShowAsync();

			if ((int)result.Id == 0)
			{
				var messageDialog2 = new MessageDialog($"NOTICE! A game of Mine Sweeper costs {Game_Hub.MineSweeperGameCost} Crystals. Are you sure?");
				messageDialog2.Commands.Add(new UICommand("Yes") { Id = 0 });
				messageDialog2.Commands.Add(new UICommand("No") { Id = 1 });
				var result2 = await messageDialog2.ShowAsync();
				if ((int)result2.Id == 1)
				{
					LoseButton_Click(sender, e);
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
			else
			{
				this.Frame.Navigate(typeof(GameMineSweeperStart));
			}
		}

		public void addMines(int[,] board)
		{
			Random rand = new Random();

			int i, j;
			int nrBombs = MAXbombcount;

			while (nrBombs != 0)
			{
				i = rand.Next(0, dimensionX);
				j = rand.Next(0, dimensionY);

				if (board[i, j] != 9)
				{
					board[i, j] = 9;
					nrBombs--;
				}
			}

		}

		public void addMinesIndicators(int[,] board)
		{
			int i, j;

			for (j = 1; j < dimensionY - 1; j++)
				if (board[0, j] != 9)
					board[0, j] = countUpperLineMines(board, j);

			for (j = 1; j < dimensionY - 1; j++)
				if (board[dimensionX - 1, j] != 9)
					board[dimensionX - 1, j] = countLastLineMines(board, j);

			for (i = 1; i < dimensionX - 1; i++)
				if (board[i, 0] != 9)
					board[i, 0] = countLeftLineMines(board, i);

			for (i = 1; i < dimensionX - 1; i++)
				if (board[i, dimensionY - 1] != 9)
					board[i, dimensionY - 1] = countRightLineMines(board, i);

			countCornerMines(board);

			for (i = 1; i < dimensionX - 1; i++)
				for (j = 1; j < dimensionY - 1; j++)
				{
					if (board[i, j] != 9)
						board[i, j] = countCenterMines(board, i, j);
				}
		}

		public int countCenterMines(int[,] board, int i, int j)
		{
			int counter = 0;

			if (board[i, j] != 9)
			{
				if (board[i - 1, j - 1] == 9)
					counter++;

				if (board[i - 1, j] == 9)
					counter++;

				if (board[i - 1, j + 1] == 9)
					counter++;

				if (board[i, j - 1] == 9)
					counter++;

				if (board[i, j + 1] == 9)
					counter++;

				if (board[i + 1, j - 1] == 9)
					counter++;

				if (board[i + 1, j] == 9)
					counter++;

				if (board[i + 1, j + 1] == 9)
					counter++;
			}

			return counter;
		}

		public int countLastLineMines(int[,] board, int j)
		{
			int counter = 0;

			if (board[dimensionX - 2, j - 1] == 9)
				counter++;

			if (board[dimensionX - 2, j + 1] == 9)
				counter++;

			if (board[dimensionX - 1, j - 1] == 9)
				counter++;

			if (board[dimensionX - 2, j] == 9)
				counter++;

			if (board[dimensionX - 1, j + 1] == 9)
				counter++;

			return counter;
		}

		public int countUpperLineMines(int[,] board, int j)
		{
			int counter = 0;

			if (board[0, j - 1] == 9)
				counter++;

			if (board[0, j + 1] == 9)
				counter++;

			if (board[1, j - 1] == 9)
				counter++;

			if (board[1, j] == 9)
				counter++;

			if (board[1, j + 1] == 9)
				counter++;

			return counter;
		}

		public int countLeftLineMines(int[,] board, int i)
		{
			int counter = 0;

			if (board[i - 1, 0] == 9)
				counter++;

			if (board[i - 1, 1] == 9)
				counter++;

			if (board[i, 1] == 9)
				counter++;

			if (board[i + 1, 1] == 9)
				counter++;

			if (board[i + 1, 0] == 9)
				counter++;

			return counter;
		}

		public int countRightLineMines(int[,] board, int i)
		{
			int counter = 0;

			if (board[i - 1, dimensionY - 1] == 9)
				counter++;

			if (board[i - 1, dimensionY - 2] == 9)
				counter++;

			if (board[i, dimensionY - 2] == 9)
				counter++;

			if (board[i + 1, dimensionY - 2] == 9)
				counter++;

			if (board[i + 1, dimensionY - 1] == 9)
				counter++;

			return counter;
		}

		public void countCornerMines(int[,] board)
		{
			int counter = 0;

			if (board[0, 0] != 9)
			{
				if (board[0, 1] == 9)
					counter++;

				if (board[1, 0] == 9)
					counter++;

				if (board[1, 1] == 9)
					counter++;

				board[0, 0] = counter;
				counter = 0;
			}

			if (board[dimensionX - 1, 0] != 9)
			{
				if (board[dimensionX - 2, 0] == 9)
					counter++;

				if (board[dimensionX - 2, 1] == 9)
					counter++;

				if (board[dimensionX - 1, 1] == 9)
					counter++;

				board[dimensionX - 1, 0] = counter;
				counter = 0;
			}

			if (board[0, dimensionY - 1] != 9)
			{
				if (board[0, dimensionY - 2] == 9)
					counter++;

				if (board[1, dimensionY - 2] == 9)
					counter++;

				if (board[1, dimensionY - 1] == 9)
					counter++;

				board[0, dimensionY - 1] = counter;
				counter = 0;
			}

			if (board[dimensionX - 1, dimensionY - 1] != 9)
			{
				if (board[dimensionX - 2, dimensionY - 2] == 9)
					counter++;

				if (board[dimensionX - 1, dimensionY - 2] == 9)
					counter++;

				if (board[dimensionX - 2, dimensionY - 1] == 9)
					counter++;

				board[dimensionX - 1, dimensionY - 1] = counter;
				counter = 0;
			}

		}

	}
}
