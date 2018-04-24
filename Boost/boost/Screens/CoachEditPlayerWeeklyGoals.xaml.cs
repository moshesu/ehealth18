using boost.Core.App;
using boost.Core.Entities;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace boost.Screens
{
	public sealed partial class CoachEditPlayerWeeklyGoals : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public readonly IGoalsHandler _IGoalsHandler;


		public CoachEditPlayerWeeklyGoals()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			welcome.Text = "Hi " + _signInFlow.GetCoach().FirstName + " \nPlease reset your player's weekly goals.";

			Goals goal = _IGoalsHandler.GetGoals((_ICoachPlayersPage.GetPlayers()[Coach_View_Players.Tag]).UserId);

			ActiveMinutes_textBox.Text = (goal.WeeklyActiveMinutes > 0) ? goal.WeeklyActiveMinutes.ToString() : "";
			Calories_textBox.Text = (goal.WeeklyCaloriesBurned > 0) ? goal.WeeklyCaloriesBurned.ToString() : "";

			ActiveMinutesReward_textBox.Text = (goal.WeeklyActiveMinutes > 0) ? goal.WeeklyActiveMinutesReward.ToString() : "";
			ActiveMinutesReward_textBox.Visibility = (goal.WeeklyActiveMinutes > 0) ? Visibility.Visible : Visibility.Collapsed;
			ActiveMinutesReward.Visibility = (goal.WeeklyActiveMinutes > 0) ? Visibility.Visible : Visibility.Collapsed;

			CaloriesReward_textBox.Text = (goal.WeeklyCaloriesBurned > 0) ? goal.WeeklyCaloriesBurnedReward.ToString() : "";
			CaloriesReward_textBox.Visibility = (goal.WeeklyCaloriesBurned > 0) ? Visibility.Visible : Visibility.Collapsed;
			CaloriesReward.Visibility = (goal.WeeklyCaloriesBurned > 0) ? Visibility.Visible : Visibility.Collapsed;




		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerWeeklyGoals));
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			if(ActiveMinutes_textBox.Text == "" && Calories_textBox.Text == "")
			{
				var messageDialog = new MessageDialog("You have'nt enter any goal, Goals will remain as they were. Are you sure?");
				messageDialog.Commands.Add(new UICommand("Yes. I am sure") { Id = 0 });
				messageDialog.Commands.Add(new UICommand("No. I want to change") { Id = 1 });
				var result = await messageDialog.ShowAsync();
				if ((int)result.Id == 0)
				{
					this.Frame.Navigate(typeof(CoachViewPlayerWeeklyGoals));
					return;
				}
				else // id == 1 
					return;
			}
			string ID = (_ICoachPlayersPage.GetPlayers())[Coach_View_Players.Tag].UserId;
			int ActiveMinutes		 = (ActiveMinutes_textBox.Text == "")		 ? -1 : int.Parse(ActiveMinutes_textBox.Text);
			int ActiveMinutesReward	 = (ActiveMinutesReward_textBox.Text == "")	 ? 0  : int.Parse(ActiveMinutesReward_textBox.Text);

			int caloriesAmount		 = (Calories_textBox.Text == "")			 ? -1 : int.Parse(Calories_textBox.Text);
			int caloriesReward		 = (CaloriesReward_textBox.Text == "")		 ? 0  : int.Parse(CaloriesReward_textBox.Text);

			_IGoalsHandler.SetWeeklyGoals(ID, ActiveMinutes , ActiveMinutesReward , caloriesAmount , caloriesReward );
			this.Frame.Navigate(typeof(CoachViewPlayerWeeklyGoals));
		}
		static public bool HandleNotDigit(TextBox textBox)
		{
			for (int i = 0; i < textBox.Text.Length; i++)
			{
				if (!char.IsNumber(textBox.Text[i]))
				{
					textBox.Text = textBox.Text.Remove(i, 1);
					return true;
				}
			}

			return false;
		}
		static public void HandlePopUpTextBox(TextBox origin, TextBlock RewardTextBlock, TextBox RewardTextBox)
		{
			if (origin.Text != "")
			{
				if (HandleNotDigit(origin))
				{
					return;
				}
				RewardTextBlock.Visibility = Visibility.Visible;
				RewardTextBox.Visibility = Visibility.Visible;
			}
			else
			{
				RewardTextBlock.Visibility = Visibility.Collapsed;
				RewardTextBox.Visibility = Visibility.Collapsed;
				RewardTextBox.Text = "";
			}
		}
		private void ActiveMinutes_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			HandlePopUpTextBox(ActiveMinutes_textBox, ActiveMinutesReward, ActiveMinutesReward_textBox);
		}

		private void Calories_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			HandlePopUpTextBox(Calories_textBox, CaloriesReward, CaloriesReward_textBox);
		}
		private void OnlyNums(object sender, TextChangedEventArgs e)
		{
			CoachEditPlayerGoals.HandleNotDigit((TextBox)sender);
		}
	}
}
