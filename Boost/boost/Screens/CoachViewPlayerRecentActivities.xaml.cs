using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Users;
using boost.Util;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost.Screens
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CoachViewPlayerRecentActivities : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();
		public SolidColorBrush CoachRewardActivityGridBackgroundColorBrush = MainPage.getCoachRewardActivityGridBackgroundColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public readonly IActivityFetcher _activityFetcher;
		public readonly IBalanceHandler _IBalanceHandler;
		Player player;
		Grid panel;
		TextBlock textBlock;
		TextBox textBox;
		Button buttonAccept;
		Button buttonCancel;
		int gridIndex;
		Grid[] grids;
		Activity[] activities;

		public CoachViewPlayerRecentActivities()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			_activityFetcher = ProgramContainer.container.GetInstance<IActivityFetcher>();
			_IBalanceHandler = ProgramContainer.container.GetInstance<IBalanceHandler>();

			player = (_ICoachPlayersPage.GetPlayers()[Coach_View_Players.Tag]);
			Init();
		}

		private void Init()
		{
			var end = DateTime.Now;
			var start = end.AddDays(-30);
			activities = _activityFetcher.GetActivitiesDuringSpan(start, end, player.UserId);
			int NumOfActivities = activities.Length;
			if (NumOfActivities == 0)
			{
				ShowMessage("No recent activities available. Tell your player to go out for a run!");
				return;
			}
			grids = new Grid[NumOfActivities];
			for (int i = 0; i < NumOfActivities; i++)
			{
				AddActivitySummaryToGrid(i);
			}
			AddPageTitle(inner, player.FirstName + "'s recent activities:");
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
			for(int i=0;i<4;i++)
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
			var activity = activities[i];
			if (activity == null)
				return;
			RowDefinition row = new RowDefinition();
			row.Height = new GridLength(9, GridUnitType.Star);
			RowDefinition row2 = new RowDefinition();
			row2.Height = new GridLength(1, GridUnitType.Star);
			inner.RowDefinitions.Add(row);
			inner.RowDefinitions.Add(row2);
			Button reward = new Button
			{
				Content = "Reward Crystals for this activity",
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
			AddTitle(grid, ActivityTypeExtensions.ToFriendlyString(activity.ActivityType), activity.DayId);

			AddTextBlockToGrid(grid, "Start Time", activity.StartTime.ToString("HH:mm:ss"), 2);
			AddTextBlockToGrid(grid, "End Time", activity.EndTime.ToString("HH:mm:ss"), 3);
			AddTextBlockToGrid(grid, "Duration", TimeUtil.GetDuration(activity.Duration).ToString(), 4);
			AddTextBlockToGrid(grid, "Total Distance", Math.Round((double)activity.TotalDistance / 100000, 2).ToString() + " KM", 5);
			AddTextBlockToGrid(grid, "Total Calories", activity.TotalCalories.ToString(), 6);
			AddTextBlockToGrid(grid, "Average Heart Rate", activity.AverageHeartRate.ToString(), 7);
			AddTextBlockToGrid(grid, "Lowest Heart Rate", activity.LowestHeartRate.ToString(), 8);
			AddTextBlockToGrid(grid, "Highest Heart Rate", activity.PeakHeartRate.ToString(), 9);
			inner.Children.Add(grid);
			Grid.SetRow(grid, 2 * i + 1);
			grids[i] = grid;
		}
		public void AddTitle(Grid grid, string activityType, DateTime DayId)
		{
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());
			TextBlock textBlock = new TextBlock
			{
				Text = $"{FirstLetterToCapital(activityType)} on {DayId.ToString("d MMM yyyy")}",
				Foreground = TextOnBackgroundColorBrush,
				FontSize = 25,
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
		public string FirstLetterToCapital(string str)
		{
			return str.Substring(0, 1).ToUpper() + str.Substring(1);
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
				var activity = activities[index];
				_IBalanceHandler.SendTransaction(amount, $"{activity.ActivityType} on {activity.DayId}", player.UserId);
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
