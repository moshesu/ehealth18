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

namespace Patient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Page_Reset();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        public void Page_Reset()
        {
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxID.Text = "";
            textBoxAge.Text = "";
            textBoxEmail.Text = "";
            textBoxPhone.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid Email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
            }
            else if (!Regex.IsMatch(textBoxPhone.Text, @"^[0-9]+$"))
            {
                errormessage.Text = "Enter a valid Phone.";
                textBoxPhone.Select(0, textBoxPhone.Text.Length);
            }
            else if (!Regex.IsMatch(textBoxID.Text, @"^[0-9]{9}$"))
            {
                errormessage.Text = "Enter a valid ID.";
                textBoxID.Select(0, textBoxID.Text.Length);
            }
            else if (!Regex.IsMatch(textBoxAge.Text, @"^[0-9]+$"))
            {
                errormessage.Text = "Enter a valid Age.";
                textBoxAge.Select(0, textBoxAge.Text.Length);
            }
            else
            {
                string firstname = textBoxFirstName.Text;
                string lastname = textBoxLastName.Text;
                string id = textBoxID.Text;
                string age = textBoxAge.Text;
                string email = textBoxEmail.Text;
                string phone = textBoxPhone.Text;
                string password = passwordBox1.Password;
                if (passwordBox1.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                }
                if (passwordBox1.Password.Length < 8)
                {
                    errormessage.Text = "Password too short, min 8 letters.";
                }
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                }
                else if (passwordBox1.Password != passwordBoxConfirm.Password)
                {
                    errormessage.Text = "Confirm password must be same as password.";
                }
                else
                {
                    errormessage.Text = "";

                    //Create an HTTP client object
                    HttpClient httpClient = new HttpClient();

                    Uri requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/UpdatePatient?code=OjFiL97RKS12DlV2lnuMdSCRhfoQay/izTPuCrbqfGZjQ76zSj2a3g==");

                    HttpStringContent stringContent = new HttpStringContent(
                        "{ \"name\": \"" + firstname + "\", " +
                        "\"last\": \"" + lastname + "\"," +
                        "\"id\": \"" + id + "\"," +
                        "\"age\": \"" + age + "\"," +
                        "\"email\": \"" + email + "\"," +
                        "\"password\": \"" + password + "\"," +
                        "\"phone\": \"" + phone + "\"}",
                        UnicodeEncoding.Utf8, "application/json");

                    try
                    {
                        HttpResponseMessage response = await httpClient.PostAsync(requestUri, stringContent);
                        errormessage.Text = await response.Content.ReadAsStringAsync();
                        if((int)response.StatusCode == 200)
                        {
                            localSettings.Values["pname"] = firstname;
                            localSettings.Values["plast"] = lastname;
                            localSettings.Values["pid"] = id;
                            localSettings.Values["page"] = age;
                            localSettings.Values["pemail"] = email;
                            localSettings.Values["pphone"] = phone;
                            localSettings.Values["ppassword"] = password;
                            localSettings.Values["pcaretakers"] = "";
                            localSettings.Values["plasthr"] = "Never";
                            localSettings.Values["preads"] = "";
                           
                            //localSettings.Values["plastLocation"] = "Never";
                            localSettings.Values["pLatitude"] ="";
                            localSettings.Values["pLongitude"] = "";
                          
                            ///////////

                            Frame.Navigate(typeof(ProfilePage));
                        }
                        
                        else
                        {
                            errormessage.Text = "email already in use";
                        }
                         
                        
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult.ToString("X") == "80072EE7")
                        {
                            errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: check your internet connection";
                        }
                        else {
                            errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                        }
                    }

                    Page_Reset();
                }
            }
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
