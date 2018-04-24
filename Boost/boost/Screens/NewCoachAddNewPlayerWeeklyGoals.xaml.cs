using boost.Core.App;
using boost.Core.Entities;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace boost.Screens
{
	public sealed partial class NewCoachAddNewPlayerWeeklyGoals : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public readonly IGoalsHandler _IGoalsHandler;


		public NewCoachAddNewPlayerWeeklyGoals()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			welcome.Text = "Hi " + _signInFlow.GetCoach().FirstName + " \nPlease set your player's weekly goals.";
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			if (ActiveMinutes_textBox.Text == "" && Calories_textBox.Text == "")
			{
				var messageDialog = new MessageDialog("You have'nt enter any goal, You could always re enter weekly goals. Continue?");
				messageDialog.Commands.Add(new UICommand("Yes. I am sure") { Id = 0 });
				messageDialog.Commands.Add(new UICommand("No. I want to change") { Id = 1 });
				var result = await messageDialog.ShowAsync();
				if ((int)result.Id == 1)
				{
					return;
				}
			}
			int ActiveMinutes = (ActiveMinutes_textBox.Text == "") ? 0 : int.Parse(ActiveMinutes_textBox.Text);
			int ActiveMinutesReward = (ActiveMinutesReward_textBox.Text == "") ? 0 : int.Parse(ActiveMinutesReward_textBox.Text);

			int caloriesAmount = (Calories_textBox.Text == "") ? 0 : int.Parse(Calories_textBox.Text);
			int caloriesReward = (CaloriesReward_textBox.Text == "") ? 0 : int.Parse(CaloriesReward_textBox.Text);


			string ID = _ICoachPlayersPage.AddNewPlayer();
			_IGoalsHandler.SetDailyGoals(ID,
			New_Coach_Add_New_Player_Goals.stepsAmount,
			New_Coach_Add_New_Player_Goals.stepsReward,
			New_Coach_Add_New_Player_Goals.sleepAmount,
			New_Coach_Add_New_Player_Goals.sleepReward,
			New_Coach_Add_New_Player_Goals.caloriesAmount,
			New_Coach_Add_New_Player_Goals.caloriesReward);

			_IGoalsHandler.SetWeeklyGoals(ID, ActiveMinutes, ActiveMinutesReward, caloriesAmount, caloriesReward);

			string msg = "User created! Please give your user the code: " + ID;
			var messageDialog3 = new MessageDialog(msg);
			messageDialog3.Commands.Add(new UICommand("OK") { Id = 0 });
			var result3 = await messageDialog3.ShowAsync();
			this.Frame.Navigate(typeof(Coach_Lobby));
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
