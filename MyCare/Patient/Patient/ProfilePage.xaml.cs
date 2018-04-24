using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Patient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();
        }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            textBlockLastHR.Text = (string)localSettings.Values["plasthr"];
            textBlockFirstName.Text = (string)localSettings.Values["pname"];
            textBlockLastName.Text = (string)localSettings.Values["plast"];
            textBlockID.Text = (string)localSettings.Values["pid"];
            textBlockAge.Text = (string)localSettings.Values["page"];
            textBlockPhone.Text = (string)localSettings.Values["pphone"];
            textBlockEmail.Text = (string)localSettings.Values["pemail"];
            textBlockCaretakers.Text = ((string)localSettings.Values["pcaretakers"]).TrimStart().Replace(' ','\n');
        }

        private void Monitor_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MonitorPage));
        }

        private void Caretaker_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddCaretakerPage));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["pemail"] = null;
            localSettings.Values["plasthr"] = null;
            localSettings.Values["pcaretakers"] = null;
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
