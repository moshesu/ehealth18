using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Progress;
using boost.Core.Entities.Users;
using boost.Util;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost.Screens
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CoachViewPlayerWeeklyGoals : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();
		public SolidColorBrush ProgressBackgroundColorBrush = MainPage.getProgressBackgroundColorBrush();
		public SolidColorBrush ProgressForegroundColorBrush = MainPage.getProgressForegroundColorBrush();
		public SolidColorBrush ProgressRectFillColorBrush = MainPage.getProgressRectFillColorBrush();


		public readonly ISignInFlow _signInFlow;
		public readonly IProgressFetcher _IProgressFetcher;
		public readonly IGoalsHandler _IGoalsHandler;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		double width = CoachViewPlayerStart.width;

		Player player;
		public CoachViewPlayerWeeklyGoals()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IProgressFetcher = ProgramContainer.container.GetInstance<IProgressFetcher>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			player = (_ICoachPlayersPage.GetPlayers())[Coach_View_Players.Tag];
			InitAllProgressBars(inner, DateTime.Now, 0);

		}

		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerStart));
		}
		public void ShowProgressBar(Grid outerGrid, string goalText, int amount, int goalAmount, int rewardAmount, int index)
		{
			Grid grid = new Grid();

			InitGrid(grid, 3, 4); //initiate the grid to have 3 rows and 4 columns

			double percent = Math.Min(1, (double)amount / goalAmount);

			TextBlock ProgressTypeTextBlock = new TextBlock // textblock of the goal's type
			{
				Text = goalText,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 20,
				Foreground = TextOnBackgroundColorBrush,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			ProgressTypeTextBlock.SetValue(Grid.RowProperty, 0);
			ProgressTypeTextBlock.SetValue(Grid.ColumnProperty, 0);
			grid.Children.Add(ProgressTypeTextBlock);

			TextBlock ProgressAmountTextBlock = new TextBlock //textblock of goal's progress (100/250)
			{
				Text = amount + "/" + goalAmount,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 20,
				Foreground = TextOnBackgroundColorBrush,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			ProgressAmountTextBlock.SetValue(Grid.RowProperty, 1);
			ProgressAmountTextBlock.SetValue(Grid.ColumnProperty, 1);
			ProgressAmountTextBlock.SetValue(Grid.ColumnSpanProperty, 3);
			grid.Children.Add(ProgressAmountTextBlock);

			Rectangle BackgroundRect = new Rectangle
			{
				Fill = ProgressBackgroundColorBrush,
				Height = 10,
				Stroke = ProgressRectFillColorBrush,
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch,
			};
			BackgroundRect.SetValue(Grid.RowProperty, 0);
			BackgroundRect.SetValue(Grid.ColumnProperty, 1);
			BackgroundRect.SetValue(Grid.ColumnSpanProperty, 3);
			grid.Children.Add(BackgroundRect);

			Rectangle ForegroundRect = new Rectangle
			{
				Fill = ProgressForegroundColorBrush,
				Height = 10,
				Width = percent * (width * 3 / 4),
				Stroke = ProgressRectFillColorBrush,
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Left,
			};
			ForegroundRect.SetValue(Grid.RowProperty, 0);
			ForegroundRect.SetValue(Grid.ColumnProperty, 1);
			ForegroundRect.SetValue(Grid.ColumnSpanProperty, 3);
			grid.Children.Add(ForegroundRect);
			if (rewardAmount > 0)
			{
				TextBlock RewardTextBlock = new TextBlock()
				{
					Text = $"Reward: {rewardAmount} Crystals",
					FontSize = 20,
					Foreground = TextOnBackgroundColorBrush,
					TextWrapping = TextWrapping.WrapWholeWords,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
				};
				RewardTextBlock.SetValue(Grid.RowProperty, 2);
				RewardTextBlock.SetValue(Grid.ColumnProperty, 0);
				RewardTextBlock.SetValue(Grid.ColumnSpanProperty, 3);
				grid.Children.Add(RewardTextBlock);
			}
			if (percent == 1)
			{
				var bmap = new BitmapImage(new Uri("ms-appx:/Screens/vi.png", UriKind.RelativeOrAbsolute));
				var img = new Image
				{
					Source = bmap,
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
					Height = 35,
					Width = 35,
				};
				img.SetValue(Grid.RowProperty, 2);
				img.SetValue(Grid.ColumnProperty, 3);
				grid.Children.Add(img);
			}

			outerGrid.RowDefinitions.Add(new RowDefinition());
			grid.SetValue(Grid.RowProperty, index);

			outerGrid.Children.Add(grid);
		}
		private void InitAllProgressBars(Grid outerGrid, DateTime date, int index)
		{
			//for each goal, check if it's defined and get its value + show visible
			int i = 0;
			string ID = player.UserId;
			WeeklyProgressSummary sum = _IProgressFetcher.GetWeekly(DateTime.Today, ID);
			Goals goals = _IGoalsHandler.GetGoals(ID);
			if (goals.WeeklyCaloriesBurned == 0 && goals.WeeklyActiveMinutes == 0)
			{
				ShowMessage("Seems like your coach didn't set any weekly goals for you.");
				return;
			}
			Grid grid = new Grid();
			grid.SetValue(Grid.RowProperty, index);
			outerGrid.RowDefinitions.Add(new RowDefinition());
			outerGrid.Children.Add(grid);

			AddTitle(grid, "Weekly Progress:", date);
			i++;
			if (goals.WeeklyActiveMinutes > 0) // if Steps is set
			{
				ShowProgressBar(grid, "Active Minutes:", sum.ActiveMinutes, goals.WeeklyActiveMinutes, (index != 0) ? 0 : goals.WeeklyActiveMinutesReward, i);
				i++;
			}
			if (goals.WeeklyCaloriesBurned > 0) // if Calories is set
			{
				ShowProgressBar(grid, "Caloriers Burned:", sum.CaloriesBurned, goals.WeeklyCaloriesBurned, (index != 0) ? 0 : goals.WeeklyCaloriesBurnedReward, i);
				i++;
			}
			grid.RowDefinitions.Add(new RowDefinition());
			TextBlock LineBreak = new TextBlock { Text = "\n" };
			LineBreak.SetValue(Grid.RowProperty, i);
			grid.Children.Add(LineBreak);
		}
		public void InitGrid(Grid grid, int rows, int columns)
		{
			for (int i = 0; i < rows; i++)
			{
				grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}
			for (int j = 0; j < columns; j++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}
		}
		public void AddTitle(Grid grid, string msg, DateTime date)
		{
			grid.RowDefinitions.Add(new RowDefinition());
			TextBlock title = new TextBlock
			{
				Text = msg + "\n" + date.StartOfWeek().ToString("d MMM yyyy") + " - " + date.StartOfWeek().AddDays(7).ToString(("d MMM yyyy")),
				Foreground = TextOnBackgroundColorBrush,
				FontSize = 30,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			title.SetValue(Grid.RowProperty, 0);
			grid.Children.Add(title);
		}
		public async void ShowMessage(string msg)
		{
			var messageDialog = new MessageDialog(msg);
			messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
			await messageDialog.ShowAsync();
			return;
		}
		private void Edit_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachEditPlayerWeeklyGoals));
		}
		private void Daily_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerGoals));
		}
	}
}
