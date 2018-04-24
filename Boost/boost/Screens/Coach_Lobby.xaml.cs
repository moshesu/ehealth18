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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace boost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Coach_Lobby : Page
    {
		public SolidColorBrush BackgroundColorBrush = MainPage.getBackgroundColorBrush();
		public SolidColorBrush TextOnBackgroundColorBrush = MainPage.getTextOnBackgroundColorBrush();
		public SolidColorBrush ButtonColorBrush = MainPage.getButtonColorBrush();
		public SolidColorBrush ButtonTextColorBrush = MainPage.getButtonTextColorBrush();
		public SolidColorBrush LogOutButtonColorBrush = MainPage.getLogOutButtonColorBrush();

		public readonly ISignInFlow _signInFlow;
		public readonly ICoachRepository _ICoachRepository;
		public readonly IUserTypeRepository _IUserTypeRepository;
		public static bool CoachAndPlayer = false;
		Coach coach;
		public Coach_Lobby()
        {
			this.InitializeComponent();

			_ICoachRepository = ProgramContainer.container.GetInstance<ICoachRepository>();
			_signInFlow = ProgramContainer.container.GetInstance<ISignInFlow>();
			_IUserTypeRepository = ProgramContainer.container.GetInstance<IUserTypeRepository>();

			CoachAndPlayer = false;
			coach = _signInFlow.GetCoach();
			if (_IUserTypeRepository.GetUserType(coach.UserId).UserType == UserType.CoachAndPlayer)
			{
				ToBeAPlayer.Content = "Switch to Player mode";
			}

			welcome.Text = "Welcome " + coach.FirstName;
		}

        private void button_update_payment_method_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Coach_Change_Credit_Card));
        }

        private void button_players_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Coach_View_Players));
        }
        private void LogOutClick(object sender, RoutedEventArgs e)
        {
			_signInFlow.SignOut();
            this.Frame.Navigate(typeof(MainPage));
        }
		
		private async void beAPlayer(object sender, RoutedEventArgs e)
		{
			if (_IUserTypeRepository.GetUserType(coach.UserId).UserType == UserType.CoachAndPlayer)
			{
				((SignInFlow)_signInFlow).SaveUserToLocalStorage(UserType.Player);
				this.Frame.Navigate(typeof(Game_Hub));
			}
			else
			{
				var messageDialog = new MessageDialog("Please notice, in order to be a player you will need a coach. Do you have one?");
				messageDialog.Commands.Add(new UICommand("Yes") { Id = 0 });
				messageDialog.Commands.Add(new UICommand("No") { Id = 1 });
				var result = await messageDialog.ShowAsync();
				if ((int)result.Id == 1)
					return;
				CoachAndPlayer = true;
				this.Frame.Navigate(typeof(New_Player));
			}
		}
	}
}
