using boost.Core.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost.Screens
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class CoachExplenationsBeforeAddingNewPlayerGoals : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();
		public readonly ISignInFlow _signInFlow;

		public CoachExplenationsBeforeAddingNewPlayerGoals()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			textBlock1.Text = $"OK {_signInFlow.GetCoach().FirstName} you will now enter Goals for your player:\n"+
								"There are two different types of goals: Daily goals and Weekly goals\n" +
								"The Daily goals are: \n" +
								"-Steps = The number of steps you want your player to walk each day.\n" +
								"-Calories Burned = The total amount of calories you want your player to burn each day.\n" +
								"-Minutes Slept = The total number of minutes you want your player to sleep each day.\n" +
								"*The 'Steps' goal is mandatory.\n" +
								"\n" +
								"The Weekly goals are\n" +
								"-Active Minutes = The total number of minutes you want your player to be active throughout the entire week.\n" +
								"-Calories Burned = The total amount of calories you want your player to burn throughout the entire week.\n" +
								"\n" +
								"For each and every of the above goals (if you choose to assign them to your player) you can grant your player with crystals, you get to decide how much!\n" +
								"Good Luck!\n" +
								"\n";
		}
		private void button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(CoachAddNewPlayer)); 
		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Coach_View_Players));
		}
	}
}
