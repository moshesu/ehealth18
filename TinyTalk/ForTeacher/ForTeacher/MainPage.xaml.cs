using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ForTeacher
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            forLogout.Visibility = Visibility.Collapsed;
            forLogin.Visibility = Visibility.Collapsed;

            if (Common.isConectet == false)
            {
                CheckUserExistAsync();
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(LoginTeacher));
            }
            else
            {
                forLogin.Visibility = Visibility.Collapsed;
                forLogout.Visibility = Visibility.Visible;
            }
        }
  

        private async void forChat_Click(object sender, RoutedEventArgs e)
        {
            if (Common.who_am_i == "")
            {
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(LoginTeacher));
            }            
            else if (Common.counter_child == 0)
            {
                await Common.GetGardenChildrenAsync();
                if (Common.counter_child == 0)
                {
                    result.Text = "Cant find any children in the garden, plase try login again later";
                }
            }
            else
            {
                Frame toChat = Window.Current.Content as Frame;
                toChat.Navigate(typeof(SendBrodcast));
            }
        }

        private async void CheckUserExistAsync()
        {

            bool success = await Common.GetUserFromFileAsync();
            if (success == false)
            {
                forLogin.Visibility = Visibility.Visible;
                forLogout.Visibility = Visibility.Collapsed;
            }
            else
            {
                forLogin.Visibility = Visibility.Collapsed;
                forLogout.Visibility = Visibility.Visible;
            }
        }

        private async void forLogOut_Click(object sender, RoutedEventArgs e)
        {
            await Common.DeleteFileAsync("userTeacher.txt");
            Common.who_am_i = "";
            Common.garden = "";
            Common.isConectet = false;
            Common.gardenChildren = null;
            Common.counter_child = 0;

            Frame toLogin = Window.Current.Content as Frame;
            toLogin.Navigate(typeof(LoginTeacher));
        }

        private void forRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame toRegister = Window.Current.Content as Frame;
            toRegister.Navigate(typeof(Registration));
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            Frame toLogin = Window.Current.Content as Frame;
            toLogin.Navigate(typeof(LoginTeacher));
        }
        
        private async void forAnalysis_Click(object sender, RoutedEventArgs e)
        {
            if (Common.who_am_i == "")
            {
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(LoginTeacher));
            }
            else if (Common.counter_child == 0)
            {
                await Common.GetGardenChildrenAsync();
                if (Common.counter_child == 0)
                {
                    result.Text = "Cant find any children in the garden, plase try login again later";
                }
                else
                {
                    Frame toAnalysis = Window.Current.Content as Frame;
                    toAnalysis.Navigate(typeof(Statistics));
                }
            }
            else
            { 
                Frame toAnalysis = Window.Current.Content as Frame;
                toAnalysis.Navigate(typeof(Statistics));
            }
        }

    }

}