using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace ForChild
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
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(loginChild));
            }
            else
            {
                forLogin.Visibility = Visibility.Collapsed;
                forLogout.Visibility = Visibility.Visible;
            }
        }

        private async void Button_Click_Friend(object sender, RoutedEventArgs e)
        {
            bool isSimilar = false;
            if (Common.who_am_i.Equals(""))
            {
                result.Text= "please log-in";
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(loginChild));
            }
            else
            {
                if (Common.myFriend.Equals(""))
                {
                    result.Text = "please add contacts";
                    Frame toAddContacts = Window.Current.Content as Frame;
                    toAddContacts.Navigate(typeof(AddUsersForChat));
                }
                isSimilar = await Common.CheckForSimilarFriendAsync();
                if (isSimilar == true)
                {
                    Frame toFriend = Window.Current.Content as Frame;
                    toFriend.Navigate(typeof(FriendPage));
                }
                else {
                    result.Text = "you have to be registerd at yout friend's app";
                }
            }
        }

        private void Button_Click_father(object sender, RoutedEventArgs e)
        {
            if (Common.who_am_i.Equals(""))
            {
                result.Text = "please log-in";
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(loginChild));
            }
            else
            {
                if (!Common.myFather.Equals(""))
                {
                    Frame toFather = Window.Current.Content as Frame;
                    toFather.Navigate(typeof(FatherPage));
                }
                else {
                    result.Text = "please add contacts";
                    Frame toAddContacts = Window.Current.Content as Frame;
                    toAddContacts.Navigate(typeof(AddUsersForChat));
                }
            }
        }

        private void Button_Click_mother(object sender, RoutedEventArgs e)
        {
            if (Common.who_am_i.Equals(""))
            {
                result.Text = "please log-in";
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(loginChild));
            }
            else
            {
                if (!Common.myMother.Equals(""))
                {
                    Frame toMather = Window.Current.Content as Frame;
                    toMather.Navigate(typeof(MotherPage));
                }
                else {
                    result.Text = "please add contacts";
                    Frame toAddContacts = Window.Current.Content as Frame;
                    toAddContacts.Navigate(typeof(AddUsersForChat));
                }
            }
        }

        private async void Button_Click_Teacher(object sender, RoutedEventArgs e)
        {
            if (Common.who_am_i.Equals(""))
            {
                result.Text = "please log-in";
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(loginChild));
            }
            else
            {
                if (!Common.myTeacher.Equals(""))
                {
                    Frame toTeacher = Window.Current.Content as Frame;
                    toTeacher.Navigate(typeof(TeacherPage));
                }
                else
                {
                    ConnectDB db = new ConnectDB();
                    Common.myTeacher = await db.GetGardenTeacher(Common.who_am_i, "Teacher");
                    if (!Common.myTeacher.Equals(""))
                    {
                        Frame toTeacher = Window.Current.Content as Frame;
                        toTeacher.Navigate(typeof(TeacherPage));
                    }
                    else
                    {
                        result.Text = "cant fild any teacher to the child garden";
                    }
                }
            }
        }

        private void forLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Common.who_am_i.Equals(""))
            {
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(loginChild));
            }
        }

        private async void forLogOut_Click(object sender, RoutedEventArgs e)
        {
                await Common.DeleteFileAsync("userChild.txt");
                await Common.DeleteFileAsync("chatWithFriend.txt");
                await Common.DeleteFileAsync("chatWithMother.txt");
                await Common.DeleteFileAsync("chatWithFather.txt");
                Common.who_am_i = "";
                Common.myFather = "";
                Common.myMother = "";
                Common.myFriend = "";
                Common.myTeacher = "";
                Common.isConectet = false;
                Common.my_num_of_msg = 0;
                Common.iStarted = true;
                Frame toMather = Window.Current.Content as Frame;
                toMather.Navigate(typeof(loginChild));
        }

        private void Button_Click_Plus(object sender, RoutedEventArgs e)
        {
            if (Common.who_am_i.Equals(""))
            {
                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(loginChild));
            }
            else
            { 
                Frame toAddUsersForChat = Window.Current.Content as Frame;
                toAddUsersForChat.Navigate(typeof(AddUsersForChat));
            }
        }

        private async void CheckUserExistAsync()
        {
            bool success = await Common.GetUserFromFileAsync();
            if (success == false)
            {
                forLogout.Visibility = Visibility.Collapsed;
                forLogin.Visibility = Visibility.Visible;

                Frame toLogin = Window.Current.Content as Frame;
                toLogin.Navigate(typeof(loginChild));
            }
            else
            {
                forLogout.Visibility = Visibility.Visible;
                forLogin.Visibility = Visibility.Collapsed;

            }
        }

    }
}
