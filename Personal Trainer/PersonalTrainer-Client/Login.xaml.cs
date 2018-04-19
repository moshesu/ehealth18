using Microsoft.WindowsAzure.MobileServices;
using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.Flows.MainFlow;
using PersonalTrainer_Client.Flows.Registration;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PersonalTrainer_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        // Define a member variable for storing the signed-in user. 
        private MobileServiceUser user;

        IMobileServiceTable<User> UsersTable = App.MobileService.GetTable<User>();


        // Define a method that performs the authentication process
        // using a Facebook sign-in. 
        private async System.Threading.Tasks.Task<bool> AuthenticateAsync()
        {
            string message;
            bool success = false;
            try
            {
                // Change 'MobileService' to the name of your MobileServiceClient instance.
                // Sign-in using Google authentication.
                user = await App.MobileService
                    .LoginAsync(MobileServiceAuthenticationProvider.Google, "personal-trainer.azurewebsites.net");
                message =
                    string.Format("You are now signed in.");
                
                success = true;
            }
            catch (InvalidOperationException)
            {
                message = "You must log in. Login Required";
            }

            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();

            return success;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Uri)
            {
                App.MobileService.ResumeWithURL(e.Parameter as Uri);
            }
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            // Login the user and then load data from the mobile app.
            if (await AuthenticateAsync())
            {
                // Switch the buttons and load items from the mobile app.
                // ButtonLogin.Visibility = Visibility.Collapsed;
                
                //await InitLocalStoreAsync(); //offline sync support.
                // await RefreshTodoItems();

                User currentUser = null;
                try
                {
                    currentUser = await UsersTable.LookupAsync(user.UserId.Substring(4));

                    var parameters = new MainDashboardParams(currentUser);

                    Frame.Navigate(typeof(MainDashboard), parameters);
                }
                catch (Exception)
                {
                    Frame.Navigate(typeof(UserDeatailsPage), user.UserId.Substring(4));
                }

            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChooseTrainingProgram), null);
        }
    }
}
