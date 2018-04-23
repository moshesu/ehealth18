using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ForTeacher
{
    public sealed partial class LoginTeacher : Page
    {
        public LoginTeacher()
        {
            this.InitializeComponent();
            loading.Visibility = Visibility.Collapsed;
        }

        private async void EnterAppAsync(object sender, RoutedEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            ConnectDB db = new ConnectDB();
            user user = await db.GetUserByMailAsync(email.Text);
            if (user == null)
            {
                loading.Visibility = Visibility.Collapsed;
                result.Text = "user doesn't exists";
            }
            else
            {
                if (user.password.Equals(password.Password.ToString()) && user.type.Equals("Teacher"))
                {
                    Common.who_am_i = email.Text;
                    Common.garden = user.gardenname;
                    Common.isConectet = true;
                    Common.garden = user.gardenname;
                    Common.GetGardenChildrenAsync();
                    try
                    {
                        Windows.Storage.StorageFolder storageFolder =
                        Windows.Storage.ApplicationData.Current.LocalFolder;
                        Windows.Storage.StorageFile userFile =
                            await storageFolder.CreateFileAsync("userTeacher.txt",
                                Windows.Storage.CreationCollisionOption.ReplaceExisting);
                        await Windows.Storage.FileIO.WriteTextAsync(userFile, email.Text
                            + "+" + password.Password.ToString() + "+" + Common.garden);
                    }
                    catch
                    {
                    }
                        loading.Visibility = Visibility.Collapsed;
                        result.Text = "welcome " + user.firstname + "!";
                        Frame toHome = Window.Current.Content as Frame;
                        toHome.Navigate(typeof(MainPage));
                }
                else if(!user.type.Equals("Teacher"))
                {
                    loading.Visibility = Visibility.Collapsed;
                    result.Text = "Type is not a teacher";
                }
                else
                {
                    loading.Visibility = Visibility.Collapsed;
                    result.Text = "incorrect password";        
                }
            }
        }

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {
            Frame toHome = Window.Current.Content as Frame;
            toHome.Navigate(typeof(MainPage));
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            Frame toRegister = Window.Current.Content as Frame;
            toRegister.Navigate(typeof(Registration));
        }
    }
}
