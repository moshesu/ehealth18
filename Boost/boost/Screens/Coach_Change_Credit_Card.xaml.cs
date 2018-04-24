using boost.Core.App;
using boost.Core.Entities.Users;
using boost.Repositories;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Coach_Change_Credit_Card : Page
	{
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachRepository _ICoachRepository;
		Coach coach;

		public Coach_Change_Credit_Card()
		{
			this.InitializeComponent();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_ICoachRepository = ProgramContainer.container.GetInstance<ICoachRepository>();
			coach = _signInFlow.GetCoach();

			credit_card_textbox.Text = "************" + coach.PaymentLastDigits;

			welcome.Text = "Hi " + _signInFlow.GetCoach().FirstName + "\nenter your new credit card number :";
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			if (credit_card_textbox.Text.Length != 16)
			{
				var messageDialog = new MessageDialog("ERROR: your credit card number must be 16 digits long");
				messageDialog.Commands.Add(new UICommand("OK") { Id = 0 });
				var result = await messageDialog.ShowAsync();
				return;
			}
			coach = _signInFlow.GetCoach();
			coach.PaymentLastDigits = credit_card_textbox.Text.Substring(12, 4);
			_ICoachRepository.SaveCoach(coach);
			this.Frame.Navigate(typeof(Coach_Lobby));
		}
		private void Back_Button_Click(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(Coach_Lobby));
		}

		private void credit_card_textbox_TextChanged(object sender, TextChangedEventArgs e)
		{
			CoachEditPlayerGoals.HandleNotDigit(credit_card_textbox);
		}
	}
}
