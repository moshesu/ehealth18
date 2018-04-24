using boost.Core.App;
using boost.Core.Entities;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using boost.Core.Entities;
using Windows.UI.Xaml.Media;
using boost.Screens;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class New_Coach_Add_New_Player_Goals : Page
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
		public New_Coach_Add_New_Player_Goals()
        {
            this.InitializeComponent();
            _signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();

			welcome.Text = "Hi " + _signInFlow.GetCoach().FirstName + " \nPlease set your player's daily goals.";
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
			if(!(Steps_textBox.Text		== "" || StepsReward_textBox.Text		!= "")
			|| !(Calories_textBox.Text	== "" || CaloriesReward_textBox.Text	!= "")
			|| !(Sleep_textBox.Text		== "" || SleepReward_textBox.Text		!= "")) //if there is a goal without a Reward
			{
				var messageDialog = new MessageDialog("You must enter Reward for each goal");
				messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				return;
			}

			if(Steps_textBox.Text == "")
			{
				var messageDialog = new MessageDialog("You must enter steps goal");
				messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				return;
			}
			

			 stepsAmount		= int.Parse(Steps_textBox.Text);
			 stepsReward		= int.Parse(StepsReward_textBox.Text);

			 sleepAmount		= (Sleep_textBox.Text == "")			? -1 : int.Parse(Sleep_textBox.Text);
			 sleepReward		= (SleepReward_textBox.Text == "")		? 0 : int.Parse(SleepReward_textBox.Text);

			 caloriesAmount	= (Calories_textBox.Text == "")			? -1 : int.Parse(Calories_textBox.Text);
			 caloriesReward	= (CaloriesReward_textBox.Text == "")	? 0 : int.Parse(CaloriesReward_textBox.Text);

			this.Frame.Navigate(typeof(NewCoachAddNewPlayerWeeklyGoals));
        }
        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
			this.Frame.Navigate(typeof(NewCoachExplenationsBeforeAddingNewPlayerGoals));
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
