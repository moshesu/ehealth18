using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Progress;
using boost.Core.Entities.Users;
using boost.Core.UserActivity;
using boost.Screens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace boost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CoachViewPlayerGoals : Page
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

		Button HistoryButton;
		Player player; 
		public CoachViewPlayerGoals()
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
			DailyProgressSummary sum = _IProgressFetcher.GetDaily(date, player.UserId);
			Goals goals = _IGoalsHandler.GetGoals(player.UserId);
			Grid grid = new Grid();
			grid.SetValue(Grid.RowProperty, index);
			outerGrid.RowDefinitions.Add(new RowDefinition());
			outerGrid.Children.Add(grid);

			AddTitle(grid, player.FirstName + "'s Daily Progress:", date);
			i++;
			if (sum.StepsTakenGoal > 0) // if Steps is set
			{
				ShowProgressBar(grid, "Steps:", sum.StepsTaken,(index == 0 ) ? goals.StepsTaken : sum.StepsTakenGoal, (index != 0) ? 0 : goals.StepsTakenReward, i);
				i++;
			}
			if (sum.CaloriesBurnedGoal > 0) // if Calories is set
			{
				ShowProgressBar(grid, "Caloriers Burned:", sum.CaloriesBurned, (index == 0) ? goals.CaloriesBurned : sum.CaloriesBurnedGoal, (index != 0) ? 0 : goals.CaloriesBurnedReward, i);
				i++;
			}
			if (sum.SleepMinutesGoal > 0) // if Sleep is set
			{
				ShowProgressBar(grid, "Minutes Slept:", sum.SleepMinutes, (index == 0) ? goals.SleepMinutes : sum.SleepMinutesGoal, (index != 0) ? 0 : goals.SleepMinutesReward, i);
				i++;
			}
			grid.RowDefinitions.Add(new RowDefinition());
			TextBlock LineBreak = new TextBlock { Text = "\n" };
			LineBreak.SetValue(Grid.RowProperty, i);
			grid.Children.Add(LineBreak);
			i++;
			AddSummary(grid, sum, i);
			i++;
			if (index == 0)
			{
				grid.RowDefinitions.Add(new RowDefinition());
				HistoryButton = new Button
				{
					Content = "View 7 days history",
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					Background = ButtonColorBrush,
					Foreground = ButtonTextColorBrush,
					Visibility = Visibility.Visible,
				};
				HistoryButton.SetValue(Grid.RowProperty, i);
				HistoryButton.Click += new RoutedEventHandler(HistoryButtonClick);
				grid.Children.Add(HistoryButton);

			}
		}

		private void HistoryButtonClick(object sender, RoutedEventArgs e)
		{
			HistoryButton.Visibility = Visibility.Collapsed;
			for (int i = 1; i < 8; i++)
			{
				InitAllProgressBars(inner, DateTime.Now.AddDays(-i), i);
			}

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
				Text = msg + "\n" + date.ToString(("d MMM yyyy")),
				Foreground = TextOnBackgroundColorBrush,
				FontSize = 30,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			title.SetValue(Grid.RowProperty, 0);
			grid.Children.Add(title);
		}
		public void AddTextBlockToGrid(Grid grid, string Text, string Value, int index)
		{
			RowDefinition row2 = new RowDefinition();
			grid.RowDefinitions.Add(row2);
			TextBlock type1 = new TextBlock
			{
				Text = Text + ": ",
				Foreground = TextOnBackgroundColorBrush,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			Border border1 = new Border
			{
				Child = type1,
				BorderThickness = new Thickness(1),
				BorderBrush = TextOnBackgroundColorBrush,
			};
			type1.VerticalAlignment = VerticalAlignment.Center;
			grid.Children.Add(border1);
			Grid.SetRow(border1, index);
			Grid.SetColumn(border1, 0);

			TextBlock type2 = new TextBlock
			{
				Text = " " + Value,
				Foreground = TextOnBackgroundColorBrush,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			Border border2 = new Border
			{
				Child = type2,
				BorderThickness = new Thickness(1),
				BorderBrush = TextOnBackgroundColorBrush,
			};
			type2.VerticalAlignment = VerticalAlignment.Center;
			grid.Children.Add(border2);
			Grid.SetRow(border2, index);
			Grid.SetColumn(border2, 1);
		}
		public void AddSummary(Grid outerGrid, DailyProgressSummary sum, int index)
		{
			outerGrid.RowDefinitions.Add(new RowDefinition());
			Grid grid = new Grid();
			InitGrid(grid, 0, 2);
			AddTextBlockToGrid(grid, "Floors climed", sum.FloorsClimbed.ToString(), 0);
			AddTextBlockToGrid(grid, "Active minutes", sum.ActiveMinutes.ToString(), 1);
			AddTextBlockToGrid(grid, "Total distance", Math.Round((double)sum.TotalDistance / 100000, 2).ToString() + " KM", 2);
			grid.SetValue(Grid.RowProperty, index);
			outerGrid.Children.Add(grid);
		}
		private void Edit_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachEditPlayerGoals));
        }
		private void Weekly_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerWeeklyGoals));
		}
	}
}
