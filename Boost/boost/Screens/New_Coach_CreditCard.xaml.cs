using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Threading.Tasks;
using boost.Core.App;
using Windows.UI.Popups;
using boost.Core.Entities.Users;
using boost.Repositories;
using boost.Screens;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class New_Coach_CreditCard : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly IGoalsHandler _IGoalsHandler;
		public readonly ICoachPlayersPage _ICoachPlayersPage;
		public readonly ICoachRepository _ICoachRepository;
		public New_Coach_CreditCard()
        {
            this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IGoalsHandler = ProgramContainer.container.GetInstance<IGoalsHandler>();
			_ICoachPlayersPage = ProgramContainer.container.GetInstance<ICoachPlayersPage>();
			_ICoachRepository = ProgramContainer.container.GetInstance<ICoachRepository>();

			welcome.Text = "Hi " + _signInFlow.GetCoach().FirstName + " \nWe're gonna need you to enter your credit card number:";
        }



        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if(credit_card_textbox.Text.Length != 16)
            {
				var messageDialog = new MessageDialog("ERROR: your credit card number must be 16 digits long");
				messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				return;
			}

			Coach coach = _signInFlow.GetCoach();
			coach.PaymentLastDigits = credit_card_textbox.Text.Substring(12, 4);
			_ICoachRepository.SaveCoach(coach);
			this.Frame.Navigate(typeof(NewCoachExplenationsBeforeAddingNewPlayerGoals));


        }
		private void credit_card_textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			CoachEditPlayerGoals.HandleNotDigit(credit_card_textbox);
		}

	}
}
