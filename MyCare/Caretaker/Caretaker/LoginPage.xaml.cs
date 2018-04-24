using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Caretaker
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

                Uri requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/FetchCaretakers?code=sTd5ugcb6rhhxXML4kE9enE2p0AXCxbizijwFyr1N197N7pmEFNFpA==&email="+email+"&password="+password);

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                    if ((int)response.StatusCode != 200)
                    {
                        errormessage.Text = "incorrect email or password";
                    }
                    else
                    {
                        var textResponse = await response.Content.ReadAsStringAsync();
                        var settings = textResponse.Trim('"').Split(',');
                        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                        localSettings.Values["cemail"] = email;
                        localSettings.Values["cpassword"] = password;
                        localSettings.Values["cname"] = settings[0];
                        localSettings.Values["clast"] = settings[1];
                        localSettings.Values["cpatients"] = settings[2];
                        Frame.Navigate(typeof(ListPage));
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
