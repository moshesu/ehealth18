using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Patient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";

               
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
            }
            else
            {
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                
                //Create an HTTP client object
                HttpClient httpClient = new HttpClient();

                Uri requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/FetchPatient?code=LBn9ZSPhn7U5ZsKYaC4ICiP5KX6v/tcpxj855db4Vh6p8lsR2FAHUA==&email=" + email +"&password="+ password);

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(requestUri);
               
                    if ((int)response.StatusCode != 200)
                    {
                        errormessage.Text = "Incorrect Email or Password";
                    }
                    else
                    {
                        var textResponse = await response.Content.ReadAsStringAsync();
                        var settings = textResponse.Trim('"').Split(',');
                        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                        localSettings.Values["pemail"] = email;
                        localSettings.Values["ppassword"] = password;
                        localSettings.Values["pname"] = settings[0];
                        localSettings.Values["plast"] = settings[1];
                        localSettings.Values["pid"] = settings[2];
                        localSettings.Values["page"] = settings[3];
                        localSettings.Values["pphone"] = settings[4];
                        localSettings.Values["pcaretakers"] = settings[5];
                        localSettings.Values["plasthr"] = settings[6];
                        localSettings.Values["preads"] = settings[7];
                  
                        localSettings.Values["pLatitude"] = settings[8];
                        localSettings.Values["pLongitude"] = settings[9];
                     //   localSettings.Values["plastLocation"] = settings[10];

                      

                        Frame.Navigate(typeof(ProfilePage));
                    }
                }
                catch (Exception ex)
                {
                    if (ex.HResult.ToString("X") == "80072EE7")
                    {
                        errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: check your internet connection";
                    }
                    else
                    {
                        errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    }
                }
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage));
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
