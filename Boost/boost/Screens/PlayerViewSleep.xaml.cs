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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost.Screens
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PlayerViewSleep : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();


		public readonly ISignInFlow _signInFlow;
		Player player;
        public PlayerViewSleep()
        {
            this.InitializeComponent();
            _signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			player = _signInFlow.GetPlayer();
			Init();
		}

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
			this.Frame.Navigate(typeof(Game_Hub));
        }
		public void AddTextBlockToGrid(Grid grid, string Text, string Value, int index)
		{
			index++;
			RowDefinition row2 = new RowDefinition();
			grid.RowDefinitions.Add(row2);
			TextBlock type1 = new TextBlock
			{
				Text = Text + ": ",
				Foreground =TextOnBackgroundColorBrush,
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
		private void Init()
		{
			SleepRepository sleepRepository = new SleepRepository();
			var end = DateTime.Now;
			var start = end.AddDays(-30);
			var sleeps = sleepRepository.GetSleepRecords(player.UserId, start, end);
			int NumOfSleeps = sleeps.Length;
			AddPageTitle(inner, "Sleep summaries:");
			if (NumOfSleeps == 0)
			{
				ShowMessage("No recent sleep records available. Go take a nap!");
				return;
			}
			for (int i = 0; i < NumOfSleeps; i++)
			{
				AddSleepSummaryToGrid(sleeps[i], i + 1);
			}
		}
		private void AddSleepSummaryToGrid(Sleep sleep , int i)
		{
			if (sleep == null)
				return;
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
			Grid.SetRow(type, 2 * i + 1);
			Grid grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			AddTitle(grid, "Sleep", sleep.DayId);
			AddTextBlockToGrid(grid, "Start Time"					, sleep.StartTime.ToString("HH:mm:ss")				, 1);
			AddTextBlockToGrid(grid, "End Time"						, sleep.EndTime.ToString("HH:mm:ss")				, 2);
			AddTextBlockToGrid(grid, "Fall Asleep Time"				, sleep.FallAsleepTime.ToString("HH:mm:ss")			, 3);
			AddTextBlockToGrid(grid, "Wake Up Time"					, sleep.WakeupTime.ToString("HH:mm:ss")				, 4);
			AddTextBlockToGrid(grid, "Duration"						, TimeUtil.GetDuration(sleep.Duration).ToString()					, 5);
			AddTextBlockToGrid(grid, "Sleep Duration"				, TimeUtil.GetDuration(sleep.SleepDuration).ToString(), 6);
			AddTextBlockToGrid(grid, "Fall Asleep Duration"			, TimeUtil.GetDuration(sleep.FallAsleepDuration).ToString(), 7);
			AddTextBlockToGrid(grid, "Awake Duration"				, TimeUtil.GetDuration(sleep.AwakeDuration).ToString(), 8);
			AddTextBlockToGrid(grid, "Total Restful Sleep Duration"	, TimeUtil.GetDuration(sleep.TotalRestfulSleepDuration).ToString(), 9);
			AddTextBlockToGrid(grid, "Total Restless Sleep Duration", TimeUtil.GetDuration(sleep.TotalRestlessSleepDuration).ToString(), 10);
			AddTextBlockToGrid(grid, "Number Of Wakeups"			, sleep.NumberOfWakeups.ToString()					, 11);
			AddTextBlockToGrid(grid, "Total Calories"				, sleep.TotalCalories.ToString()					, 12);
			AddTextBlockToGrid(grid, "Resting Heart Rate"			, sleep.RestingHeartRate.ToString()					, 13);
			AddTextBlockToGrid(grid, "Average Heart Rate"			, sleep.AverageHeartRate.ToString()					, 14);
			AddTextBlockToGrid(grid, "Lowest Heart Rate"			, sleep.LowestHeartRate.ToString()					, 15);
			AddTextBlockToGrid(grid, "Highest Heart Rate"			, sleep.PeakHeartRate.ToString()					, 16);
			inner.Children.Add(grid);
			Grid.SetRow(grid, 2 * i);
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
		public async void ShowMessage(string msg)
		{
			var messageDialog = new MessageDialog(msg);
			messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
			await messageDialog.ShowAsync();
			return;
		}
	}
}
