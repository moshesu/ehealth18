using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ForParent
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            forLogout.Visibility = Visibility.Collapsed;
            forLogin.Visibility = Visibility.Visible;

            if (Common.isConectet == false)
            {
                CheckUserExistAsync();
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
                toLogin.Navigate(typeof(LoginParent));
            }
            else if (Common.myChild.IndexOf('@') <= 0)
            {
                Common.myChild = await Common.GetParentContactAsync();
                if(Common.myChild.IndexOf('@') <= 0)
                {
                    result.Text = "child didnt insert the parent has is contact";
                }
                else
                {
                    //login again for update from db
                    Frame toLogin = Window.Current.Content as Frame;
                    toLogin.Navigate(typeof(LoginParent));
                }
            }
            else
            {
                Frame toChat = Window.Current.Content as Frame;
                toChat.Navigate(typeof(Chat));
            }
        }

        private async void CheckUserExistAsync()
        {
            bool success = await Common.GetUserFromFileAsync();
            if(success == false)
            {
                forLogin.Visibility = Visibility.Visible;
                forLogout.Visibility = Visibility.Collapsed;
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(LoginParent));
            }
            else
            {
                forLogout.Visibility = Visibility.Visible;
                forLogin.Visibility = Visibility.Collapsed;
            }
        }

        private void forRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame toRegister = Window.Current.Content as Frame;
            toRegister.Navigate(typeof(Registration));
        }

        private async void forLogin_Click(object sender, RoutedEventArgs e)
        {
            Frame toLogin = Window.Current.Content as Frame;
            toLogin.Navigate(typeof(LoginParent));
        }

        private async void forLogOut_Click(object sender, RoutedEventArgs e)
        {
            await Common.DeleteFileAsync("userParent.txt");
            await Common.DeleteFileAsync("chatWithChild.txt");
            Common.who_am_i = "";
            Common.isConectet = false;
            Common.myChild = "";
            forLogout.Visibility = Visibility.Collapsed;
            forLogin.Visibility = Visibility.Visible;
            Frame toLogin = Window.Current.Content as Frame;
            toLogin.Navigate(typeof(LoginParent));        
        }
        private async void forAnalysis_Click(object sender, RoutedEventArgs e)
        {
            if (Common.who_am_i == "")
            {
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(LoginParent));
            }
            else if (Common.myChild.IndexOf('@') <= 0)
            {
                Common.myChild = await Common.GetParentContactAsync();
                if (Common.myChild.IndexOf('@') <= 0)
                {
                    result.Text = "child didnt insert the parent has is contact";
                }
                else
                {
                    Frame toAnalysis = Window.Current.Content as Frame;
                    toAnalysis.Navigate(typeof(Analysis));
                }
            }
            else
            {
                Frame toAnalysis = Window.Current.Content as Frame;
                toAnalysis.Navigate(typeof(Analysis));
            }
        }
    }
}
