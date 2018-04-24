using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using boost.Core.Entities;
using boost.Screens;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
	public sealed partial class CoachEditPlayerGoals : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public readonly IGoalsHandler _IGoalsHandler;
		Player player;

		public CoachEditPlayerGoals()
		{
			this.InitializeComponent();

			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			player = (_ICoachPlayersPage.GetPlayers())[Coach_View_Players.Tag];
			InitializeTextBoxes();		
		}
		private async void InitializeTextBoxes()
		{
			welcome.Text = "Hi " + _signInFlow.GetCoach().FirstName + " \nPlease reset your player's daily goals.";

			Goals goal = _IGoalsHandler.GetGoals(player.UserId);
			if(goal == null)
			{
				var messageDialog = new MessageDialog("Your player seem to have no goals");
				messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				return;
			}
			Steps_textBox.Text					= (goal.StepsTaken > 0)		? goal.StepsTaken.ToString() : "";
			StepsReward_textBox.Text			= (goal.StepsTaken > 0)		? goal.StepsTakenReward.ToString() : "";
			StepsReward_textBox.Visibility		= (goal.StepsTaken > 0)		? Visibility.Visible	: Visibility.Collapsed;
			StepsReward.Visibility				= (goal.StepsTaken > 0)		? Visibility.Visible	: Visibility.Collapsed;

			Calories_textBox.Text				= (goal.CaloriesBurned > 0) ? goal.CaloriesBurned.ToString() : "";
			CaloriesReward_textBox.Text			= (goal.CaloriesBurned > 0) ? goal.CaloriesBurnedReward.ToString() : "";
			CaloriesReward_textBox.Visibility	= (goal.CaloriesBurned > 0) ? Visibility.Visible : Visibility.Collapsed;
			CaloriesReward.Visibility			= (goal.CaloriesBurned > 0) ? Visibility.Visible : Visibility.Collapsed;

			Sleep_textBox.Text					= (goal.SleepMinutes > 0)	? goal.SleepMinutes.ToString() : "";
			SleepReward_textBox.Text			= (goal.SleepMinutes > 0)	? goal.SleepMinutesReward.ToString() : "";
			SleepReward_textBox.Visibility		= (goal.SleepMinutes > 0)	? Visibility.Visible : Visibility.Collapsed;
			SleepReward.Visibility				= (goal.SleepMinutes > 0)	? Visibility.Visible : Visibility.Collapsed;
		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachViewPlayerGoals));
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			if (!(Steps_textBox.Text == "" || StepsReward_textBox.Text != "")
			|| !(Calories_textBox.Text == "" || CaloriesReward_textBox.Text != "")
			|| !(Sleep_textBox.Text == "" || SleepReward_textBox.Text != "")) //if there is a goal without a Reward
			{
				var messageDialog = new MessageDialog("You must enter Reward for each goal");
				messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				return;
			}

			if (Steps_textBox.Text == "")
			{
				var messageDialog = new MessageDialog("You must enter steps goal");
				messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				return;
			}
			string ID = player.UserId;
			int stepsAmount = int.Parse(Steps_textBox.Text);
			int stepsReward = int.Parse(StepsReward_textBox.Text);
			int sleepAmount = (Sleep_textBox.Text == "") ? 0 : int.Parse(Sleep_textBox.Text);
			int sleepReward = (SleepReward_textBox.Text == "") ? 0 : int.Parse(SleepReward_textBox.Text);
			int caloriesAmount = (Calories_textBox.Text == "") ? 0 : int.Parse(Calories_textBox.Text);
			int caloriesReward = (CaloriesReward_textBox.Text == "") ? 0 : int.Parse(CaloriesReward_textBox.Text);

			_IGoalsHandler.SetDailyGoals(ID, stepsAmount, stepsReward, sleepAmount, sleepReward, caloriesAmount, caloriesReward);
			this.Frame.Navigate(typeof(CoachViewPlayerGoals));
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
		private void Steps_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			HandlePopUpTextBox(Steps_textBox, StepsReward, StepsReward_textBox);
		}

		private void Calories_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			HandlePopUpTextBox(Calories_textBox, CaloriesReward, CaloriesReward_textBox);
		}

		private void Sleep_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			HandlePopUpTextBox(Sleep_textBox, SleepReward, SleepReward_textBox);
		}
		private void OnlyNums(object sender, TextChangedEventArgs e)
		{
			CoachEditPlayerGoals.HandleNotDigit((TextBox)sender);
		}
	}
}
