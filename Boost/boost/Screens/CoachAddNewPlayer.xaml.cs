using boost.Core.App;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace boost.Screens
{
	public sealed partial class CoachAddNewPlayer : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly IGoalsHandler _IGoalsHandler;
		public readonly ICoachPlayersPage _ICoachPlayersPage;

		public static int stepsAmount;
		public static int stepsReward;

		public static int sleepAmount;
		public static int sleepReward;

		public static int caloriesAmount;
		public static int caloriesReward;
		public CoachAddNewPlayer()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();

			welcome.Text = "Hi " + _signInFlow.GetCoach().FirstName + " \nPlease set your player's goals.";
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			if (!(Steps_textBox.Text == "" || StepsReward_textBox.Text != "")
			|| !(Calories_textBox.Text == "" || CaloriesReward_textBox.Text != "")
			|| !(Sleep_textBox.Text == "" || SleepReward_textBox.Text != "")) //if there is a goal without a Reward
			{
				var messageDialog1 = new MessageDialog("You must enter Reward for each goal");
				messageDialog1.Commands.Add(new UICommand("OK") { Id = 0 });
				var result1 = await messageDialog1.ShowAsync();
				return;
			}

			if (Steps_textBox.Text == "")
			{
				var messageDialog2 = new MessageDialog("You must enter steps goal");
				messageDialog2.Commands.Add(new UICommand("OK") { Id = 0 });
				var result2 = await messageDialog2.ShowAsync();
				return;
			}
			string ID = _ICoachPlayersPage.AddNewPlayer();

			stepsAmount = int.Parse(Steps_textBox.Text);
			stepsReward = int.Parse(StepsReward_textBox.Text);

			sleepAmount = (Sleep_textBox.Text == "") ? -1 : int.Parse(Sleep_textBox.Text);
			sleepReward = (SleepReward_textBox.Text == "") ? 0 : int.Parse(SleepReward_textBox.Text);

			caloriesAmount = (Calories_textBox.Text == "") ? -1 : int.Parse(Calories_textBox.Text);
			caloriesReward = (CaloriesReward_textBox.Text == "") ? 0 : int.Parse(CaloriesReward_textBox.Text);

			this.Frame.Navigate(typeof(CoachAddNewPlayerWeeklyGoals));
		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachExplenationsBeforeAddingNewPlayerGoals));
		}

		private void Steps_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			CoachEditPlayerGoals.HandlePopUpTextBox(Steps_textBox, StepsReward, StepsReward_textBox);
		}

		private void Calories_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			CoachEditPlayerGoals.HandlePopUpTextBox(Calories_textBox, CaloriesReward, CaloriesReward_textBox);
		}

		private void Sleep_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			CoachEditPlayerGoals.HandlePopUpTextBox(Sleep_textBox, SleepReward, SleepReward_textBox);
		}
		private void OnlyNums(object sender, TextChangedEventArgs e)
		{
			CoachEditPlayerGoals.HandleNotDigit((TextBox)sender);
		}
	}
}
