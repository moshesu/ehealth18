using boost.Cloud.AzureDatabase;
using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Users;
using boost.Util;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace boost.Screens
{
	public sealed partial class CoachViewPlayerSleep : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();
		public SolidColorBrush CoachRewardActivityGridBackgroundColorBrush = MainPage.getCoachRewardActivityGridBackgroundColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public readonly ISleepFetcher _sleepFetcher;
		public readonly IBalanceHandler _IBalanceHandler;
		Player player;
		Grid panel;
		TextBlock textBlock;
		TextBox textBox;
		Button buttonAccept;
		Button buttonCancel;
		int gridIndex;
		Grid[] grids;
		Sleep[] sleeps;

		public CoachViewPlayerSleep()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			_sleepFetcher = ProgramContainer.container.GetInstance<ISleepFetcher>();
			_IBalanceHandler = ProgramContainer.container.GetInstance<IBalanceHandler>();

			player = (_ICoachPlayersPage.GetPlayers()[Coach_View_Players.Tag]);
			Init();
		}

		private void Init()
		{
			SleepRepository sleepRepository = new SleepRepository();
			var end = DateTime.Now;
			var start = end.AddDays(-30);
			sleeps = _sleepFetcher.GetSleepDuringSpan(start, end, player.UserId);
			int NumOfSleeps = sleeps.Length;
			AddPageTitle(inner, player.FirstName + "'s sleep summaries:");
			if (NumOfSleeps == 0)
			{
				ShowMessage("No recent sleep records available. Tell your player to take a nap!");
				return;
			}
			grids = new Grid[NumOfSleeps];
			for (int i = 0; i < NumOfSleeps; i++)
			{
				AddActivitySummaryToGrid(i);
			}
			panel = new Grid
			{
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				Background = CoachRewardActivityGridBackgroundColorBrush,
				Tag = -1, //current index
				Name = "RewardPanel",
			};
			textBlock = new TextBlock
			{
				Text = "How many crystals would you like to give to your user?",
				FontSize = 15,
			};
			textBox = new TextBox
			{
				Name = "RewardTextBox",
			};
			textBox.TextChanged += new TextChangedEventHandler(RewardTextBoxChanged);
			buttonAccept = new Button
			{
				Content = "Confirm",
				Background = MainPage.getButtonColorBrush(),
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				FontSize = 15,
			};
			buttonCancel = new Button
			{
				Content = "Cancel",
				Background = LogOutButtonColorBrush,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				FontSize = 15,
			};
			buttonAccept.Click += new RoutedEventHandler(Accept);
			buttonCancel.Click += new RoutedEventHandler(Cancel);
			for (int i = 0; i < 4; i++)
			{
				RowDefinition row2 = new RowDefinition();
				row2.Height = new GridLength(1, GridUnitType.Star);
				panel.RowDefinitions.Add(row2);
			}
			panel.Children.Add(textBlock);
			textBlock.SetValue(Grid.RowProperty, 0);
			panel.Children.Add(textBox);
			textBox.SetValue(Grid.RowProperty, 1);
			panel.Children.Add(buttonAccept);
			buttonAccept.SetValue(Grid.RowProperty, 2);
			panel.Children.Add(buttonCancel);
			buttonCancel.SetValue(Grid.RowProperty, 3);

			panel.SetValue(Grid.RowSpanProperty, 20);
			panel.SetValue(Grid.RowProperty, 2);
			panel.SetValue(Grid.ColumnSpanProperty, 2);
			panel.Visibility = Visibility.Collapsed;
		}

		private void AddActivitySummaryToGrid(int i)
		{
			var sleep = sleeps[i];
			if (sleep == null)
				return;
			RowDefinition row = new RowDefinition();
			row.Height = new GridLength(9, GridUnitType.Star);
			RowDefinition row2 = new RowDefinition();
			row2.Height = new GridLength(1, GridUnitType.Star);
			inner.RowDefinitions.Add(row);
			inner.RowDefinitions.Add(row2);
			Button reward = new Button
			{
				Content = "Reward Crystals for this sleep",
				Background = ButtonColorBrush,
				Foreground = ButtonTextColorBrush,
				HorizontalAlignment = HorizontalAlignment.Center,
				FontSize = 15,
				Tag = i,
			};
			reward.Click += new RoutedEventHandler(RewardActivity);
			reward.VerticalAlignment = VerticalAlignment.Center;
			inner.Children.Add(reward);
			Grid.SetRow(reward, 2 * i + 2);
			Grid grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			AddTitle(grid, "Sleep", sleep.DayId);
			AddTextBlockToGrid(grid, "Start Time", sleep.StartTime.ToString("HH:mm:ss"), 1);
			AddTextBlockToGrid(grid, "End Time", sleep.EndTime.ToString("HH:mm:ss"), 2);
			AddTextBlockToGrid(grid, "Fall Asleep Time", sleep.FallAsleepTime.ToString("HH:mm:ss"), 3);
			AddTextBlockToGrid(grid, "Wake Up Time", sleep.WakeupTime.ToString("HH:mm:ss"), 4);
			AddTextBlockToGrid(grid, "Duration", TimeUtil.GetDuration(sleep.Duration).ToString(), 5);
			AddTextBlockToGrid(grid, "Sleep Duration", TimeUtil.GetDuration(sleep.SleepDuration).ToString(), 6);
			AddTextBlockToGrid(grid, "Fall Asleep Duration", TimeUtil.GetDuration(sleep.FallAsleepDuration).ToString(), 7);
			AddTextBlockToGrid(grid, "Awake Duration", TimeUtil.GetDuration(sleep.AwakeDuration).ToString(), 8);
			AddTextBlockToGrid(grid, "Total Restful Sleep Duration", TimeUtil.GetDuration(sleep.TotalRestfulSleepDuration).ToString(), 9);
			AddTextBlockToGrid(grid, "Total Restless Sleep Duration", TimeUtil.GetDuration(sleep.TotalRestlessSleepDuration).ToString(), 10);
			AddTextBlockToGrid(grid, "Number Of Wakeups", sleep.NumberOfWakeups.ToString(), 11);
			AddTextBlockToGrid(grid, "Total Calories", sleep.TotalCalories.ToString(), 12);
			AddTextBlockToGrid(grid, "Resting Heart Rate", sleep.RestingHeartRate.ToString(), 13);
			AddTextBlockToGrid(grid, "Average Heart Rate", sleep.AverageHeartRate.ToString(), 14);
			AddTextBlockToGrid(grid, "Lowest Heart Rate", sleep.LowestHeartRate.ToString(), 15);
			AddTextBlockToGrid(grid, "Highest Heart Rate", sleep.PeakHeartRate.ToString(), 16);
			inner.Children.Add(grid);
			Grid.SetRow(grid, 2 * i + 1);
			grids[i] = grid;
		}

		public void AddTextBlockToGrid(Grid grid, string Text, string Value, int index)
		{
			index++;
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
		public void AddTitle(Grid grid, string activityType, DateTime DayId)
		{
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());
			TextBlock textBlock = new TextBlock
			{
				Text = $"{activityType} on {DayId.ToString("d MMM yyyy")}",
				Foreground = TextOnBackgroundColorBrush,
				FontSize = 30,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			textBlock.SetValue(Grid.RowProperty, 0);
			textBlock.SetValue(Grid.RowSpanProperty, 2);
			textBlock.SetValue(Grid.ColumnProperty, 0);
			textBlock.SetValue(Grid.ColumnSpanProperty, 2);
			grid.Children.Add(textBlock);
		}
		public void AddPageTitle(Grid grid, string msg)
		{
			grid.RowDefinitions.Add(new RowDefinition());
			TextBlock textBlock = new TextBlock
			{
				Text = msg,
				Foreground = TextOnBackgroundColorBrush,
				FontSize = 30,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center,
				TextWrapping = TextWrapping.WrapWholeWords,
			};
			textBlock.SetValue(Grid.RowProperty, 0);
			grid.Children.Add(textBlock);
		}
		private void RewardActivity(object sender, RoutedEventArgs e)
		{
			int prevIndex = (int)panel.Tag;
			if (prevIndex != -1)
				grids[prevIndex].Children.Remove(panel);
			textBox.Text = "";
			int index = (int)((Button)sender).Tag;
			buttonAccept.Tag = index;
			buttonCancel.Tag = index;
			panel.Tag = index;
			grids[index].Children.Add(panel);
			panel.Visibility = Visibility.Visible;

		}
		private void Cancel(object sender, RoutedEventArgs e)
		{
			grids[(int)((Button)sender).Tag].Children.Remove(panel);
			panel.Tag = -1;
			panel.Visibility = Visibility.Collapsed;
		}
		private void Accept(object sender, RoutedEventArgs e)
		{
			int index = (int)((Button)sender).Tag;
			if (textBox.Text != "")
			{
				int amount = int.Parse(textBox.Text);
				ShowMessage($"{player.FirstName} has been rewarded with {amount} crystals.");
				var activity = sleeps[index];
				_IBalanceHandler.SendTransaction(amount, $"sleep on {activity.DayId}", player.UserId);
			}
			grids[index].Children.Remove(panel);
			panel.Tag = -1;
			panel.Visibility = Visibility.Collapsed;
		}
		private void RewardTextBoxChanged(object sender, RoutedEventArgs e)
		{
			CoachEditPlayerGoals.HandleNotDigit(textBox);
		}

		public async void ShowMessage(string msg)
		{
			var messageDialog = new MessageDialog(msg);
			messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
			await messageDialog.ShowAsync();
			return;
		}

		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerStart));
		}
	}
}
