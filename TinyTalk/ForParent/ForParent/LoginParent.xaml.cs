using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ForParent
{
    public sealed partial class LoginParent : Page
    {
        public LoginParent()
        {
            this.InitializeComponent();
            loading.Visibility = Visibility.Collapsed;
        }

        private async void EnterAppAsync(object sender, RoutedEventArgs e)
        {
            loading.Visibility = Visibility.Visible;
            result.Text = "";
            string Email = email.Text;
            string Password = password.Password.ToString();
            ConnectDB db = new ConnectDB();
            user user = await db.GetUserByMailAsync(Email);
            if (user == null)
            {
                loading.Visibility = Visibility.Collapsed;
                result.Text = "user doesn't exists";
            }
            else
            {
                if (user.password.Equals(Password) && user.type.ToLower().Equals("parent"))
                {
                    Common.who_am_i = Email;
                    Common.isConectet = true;
                    string childResult = await Common.GetParentContactAsync();
                    
                    if(childResult.IndexOf('@')> 0)
                    {
                        Common.myChild = childResult;
                        try
                        {
                            Windows.Storage.StorageFolder storageFolder =
                            Windows.Storage.ApplicationData.Current.LocalFolder;
                            Windows.Storage.StorageFile userFile =
                                await storageFolder.CreateFileAsync("userParent.txt",
                                    Windows.Storage.CreationCollisionOption.ReplaceExisting);
                            await Windows.Storage.FileIO.WriteTextAsync(userFile, Email
                                + "+" + Password + "+" + childResult);
                        }
                        catch
                        {
                        }
                        loading.Visibility = Visibility.Collapsed;
                        result.Text = "welcome " + user.firstname + "!";
                        Frame toMainPage = Window.Current.Content as Frame;
                        toMainPage.Navigate(typeof(MainPage));
                    }
                    else
                    {
                        result.Text = childResult;
                    }
                }
                else if(!user.type.Equals("Parent"))
                {
                    loading.Visibility = Visibility.Collapsed;
                    result.Text = "The type is not Parent";
                }
                else
                {
                    loading.Visibility = Visibility.Collapsed;
                    result.Text = "incorrect password";
                }
            }
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            Frame toRegister = Window.Current.Content as Frame;
            toRegister.Navigate(typeof(Registration));
        }

        private void Button_Click_back(object sender, RoutedEventArgs e)
        {
            Frame toHome = Window.Current.Content as Frame;
            toHome.Navigate(typeof(MainPage));
        }
    }
}
