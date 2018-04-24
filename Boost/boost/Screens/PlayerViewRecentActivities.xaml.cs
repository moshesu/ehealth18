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

	public sealed partial class PlayerViewRecentActivities : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();


		public readonly ISignInFlow _signInFlow;
		public readonly IActivityFetcher _activityFetcher;
		Player player;
		public PlayerViewRecentActivities()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_activityFetcher = ProgramContainer.container.GetInstance<IActivityFetcher>();
			player = _signInFlow.GetPlayer();
			Init();
		}

		private void Init()
		{
			var end = DateTime.Now;
			var start = end.AddDays(-30);
			var activities = _activityFetcher.GetActivitiesDuringSpan(start, end, player.UserId);
			int NumOfActivities = activities.Length;
			if (NumOfActivities == 0)
			{
				ShowMessage("No recent activities available. Go out for a run!");
				return;
			}
			AddPageTitle(inner, "Recent activities:");
			int index = 0;
			for (int i = 0; i < NumOfActivities; i++)
			{
				if (activities[i] == null || activities[i].DayId < new DateTime(1980, 1, 1))
					continue;
				AddActivitySummaryToGrid(activities[i], index + 1);
				index++;
			}
		}

		private void AddActivitySummaryToGrid(Activity activity, int i)
		{
			
			RowDefinition row = new RowDefinition();
			row.Height = new GridLength(9, GridUnitType.Star);
			RowDefinition row2 = new RowDefinition();
			row2.Height = new GridLength(1, GridUnitType.Star);
			inner.RowDefinitions.Add(row);
			inner.RowDefinitions.Add(row2);
			TextBlock type = new TextBlock
			{
				Text = "\n"
			};
			type.VerticalAlignment = VerticalAlignment.Center;
			inner.Children.Add(type);
			type.SetValue(Grid.RowProperty, 2 * i + 1);
			//Grid.SetRow(type, 2 * i + 1);
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
			grid.SetValue(Grid.RowProperty, 2 * i);
			//Grid.SetRow(grid, 2 * i);
		}
		public void AddTitle(Grid grid, string activityType, DateTime DayId )
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

		public async void ShowMessage(string msg)
		{
			var messageDialog = new MessageDialog(msg);
			messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
			await messageDialog.ShowAsync();
			return;
		}

		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Game_Hub));
		}
	}
}
