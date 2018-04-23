using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ForChild
{
    public sealed partial class AddUsersForChat : Page
    {
        public AddUsersForChat()
        {
            this.InitializeComponent();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            string[] emails = new string[5];
            emails[0] = "Child";
            emails[1] = Common.who_am_i;
            emails[2] = TextBoxFather.Text;
            emails[3] = TextBoxMother.Text;
            emails[4] = TextBoxFriend.Text;
            
            result.Text = await Common.AddUserChatContact(emails);
            if (result.Text.Equals("success"))
            {
                Frame toHome = Window.Current.Content as Frame;
                toHome.Navigate(typeof(MainPage));
            }
        }

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {
            Frame toHome = Window.Current.Content as Frame;
            toHome.Navigate(typeof(MainPage));
        }

    }
}
