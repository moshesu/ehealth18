using boost.Core.App;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class New_Coach_Add_New_Player : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly IGoalsHandler _IGoalsHandler;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public New_Coach_Add_New_Player()
        {
            this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>(); welcome.Text = "Hi " + _signInFlow.GetCoach().FirstName + " \nTime to add your first Player. \nEnter your player's email and we will do the rest.";
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if(email_textbox.Text != "" )
            {
				//_ICoachPlayersPage.AddNewPlayer();
                this.Frame.Navigate(typeof(New_Coach_Add_New_Player_Goals));
            }
			else
			{
				var messageDialog = new MessageDialog("You must enter a valid email address");
				messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				return;
			}
        }
        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(New_Coach_CreditCard));
        }
    }
}
