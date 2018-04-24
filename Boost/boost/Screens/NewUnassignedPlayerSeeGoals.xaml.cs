using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Users;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace boost
{
	public sealed partial class NewUnassignedPlayerSeeGoals : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly IProgressFetcher _IProgressFetcher;
		public readonly IGoalsHandler _IGoalsHandler;
		public readonly ISignInFlow _signInFlow;

		Player player;
		public NewUnassignedPlayerSeeGoals()
        {
            this.InitializeComponent();
			_IProgressFetcher = ProgramContainer.container.GetInstance<IProgressFetcher>();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();

			player = _signInFlow.GetPlayer();

			welcome.Text = $"OK {player.FirstName}, these are the goals your coach assigned you!";
			InitAllProgressBars(null,null);
        }
        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewUnassignedPlayerExplenations));
        }
		private void InitAllProgressBars(object sender, RoutedEventArgs e)
        {
			InitGrid(inner, 2, 0);
			AddDailyGoals();
			AddWeeklyGoals();
        }
		private void AddDailyGoals()
		{
			int i = 1;
			Goals goals = _IGoalsHandler.GetGoals(player.UserId);
			Grid grid = new Grid();
			grid.SetValue(Grid.RowProperty, 0);
			inner.Children.Add(grid);
			InitGrid(grid, 2, 3);
			AddTitle(grid, "Daily Goals:");
			AddTextBlockToGrid(grid, "Goal Type:", i, 0);
			AddTextBlockToGrid(grid, "Goal:", i, 1);
			AddTextBlockToGrid(grid, "Reward:", i, 2);
			i++;
			if (goals.StepsTaken > 0) // if steps is set
			{
				grid.RowDefinitions.Add(new RowDefinition());
				AddTextBlockToGrid(grid, "Steps:", i, 0);
				AddTextBlockToGrid(grid, goals.StepsTaken.ToString(), i, 1);
				AddTextBlockToGrid(grid, goals.StepsTakenReward.ToString() + " Crystals", i, 2);
				i++;
			}
			if (goals.CaloriesBurned > 0) // if Calories is set
			{
				grid.RowDefinitions.Add(new RowDefinition());
				AddTextBlockToGrid(grid, "Calories Burned:", i, 0);
				AddTextBlockToGrid(grid, goals.CaloriesBurned.ToString(), i, 1);
				AddTextBlockToGrid(grid, goals.CaloriesBurnedReward.ToString() + " Crystals", i, 2);
				i++;
			}
			if (goals.SleepMinutes > 0) // if Sleep is set
			{
				grid.RowDefinitions.Add(new RowDefinition());
				AddTextBlockToGrid(grid, "Sleep Minutes:", i, 0);
				AddTextBlockToGrid(grid, goals.SleepMinutes.ToString(), i, 1);
				AddTextBlockToGrid(grid, goals.SleepMinutesReward.ToString() + " Crystals", i, 2);
				i++;
			}
		}
		private void AddWeeklyGoals()
		{
			int i = 1;
			Goals goals = _IGoalsHandler.GetGoals(player.UserId);
			Grid grid = new Grid();
			grid.SetValue(Grid.RowProperty, 1);
			inner.Children.Add(grid);
			InitGrid(grid, 2, 3);
			AddTitle(grid, "Weekly Goals:");
			
			i++;
			if (goals.WeeklyActiveMinutes > 0) // if steps is set
			{
				grid.RowDefinitions.Add(new RowDefinition());
				AddTextBlockToGrid(grid, "Steps:", i, 0);
				AddTextBlockToGrid(grid, goals.WeeklyActiveMinutes.ToString(), i, 1);
				AddTextBlockToGrid(grid, goals.WeeklyActiveMinutesReward.ToString() + " Crystals", i, 2);
				i++;
			}
			if (goals.WeeklyCaloriesBurned > 0) // if Calories is set
			{
				grid.RowDefinitions.Add(new RowDefinition());
				AddTextBlockToGrid(grid, "Calories Burned:", i, 0);
				AddTextBlockToGrid(grid, goals.WeeklyCaloriesBurned.ToString(), i, 1);
				AddTextBlockToGrid(grid, goals.WeeklyCaloriesBurnedReward.ToString() + " Crystals", i, 2);
				i++;
			}
			if(i != 2)
			{
				AddTextBlockToGrid(grid, "Goal Type:", 1, 0);
				AddTextBlockToGrid(grid, "Goal:", 1, 1);
				AddTextBlockToGrid(grid, "Reward:", 1, 2);
			}
			else
			{
				TextBlock type1 = new TextBlock
				{
					Text = "No Weekly goals assigned!",
					Foreground = TextOnBackgroundColorBrush,
					TextWrapping = TextWrapping.WrapWholeWords,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
				};
				grid.Children.Add(type1);
				Grid.SetRow(type1, 1);
				Grid.SetColumnSpan(type1, 3);
			}
		}
		private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Game_Hub));
        }

		public void AddTextBlockToGrid(Grid grid, string Text, int row, int col)
		{
			TextBlock type1 = new TextBlock
			{
				Text = Text,
				Foreground = TextOnBackgroundColorBrush,
				TextWrapping = TextWrapping.WrapWholeWords,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
			};
			Border border1 = new Border
			{
				Child = type1,
				BorderThickness = new Thickness(1),
				BorderBrush = TextOnBackgroundColorBrush,
			};
			grid.Children.Add(border1);
			Grid.SetRow(border1, row);
			Grid.SetColumn(border1, col);
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
		private void AddTitle(Grid grid, string msg)
		{
			grid.RowDefinitions.Add(new RowDefinition());
			TextBlock title = new TextBlock
			{
				Text = msg,
				Foreground = TextOnBackgroundColorBrush,
				TextWrapping = TextWrapping.WrapWholeWords,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				FontSize = 30,
			};
			grid.Children.Add(title);
			Grid.SetRow(title, 0);
			Grid.SetColumnSpan(title, 3);
		}
	}
}
