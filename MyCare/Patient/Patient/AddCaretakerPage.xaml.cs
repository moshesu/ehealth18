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
    public sealed partial class AddCaretakerPage : Page
    {
        public AddCaretakerPage()
        {
            InitializeComponent();
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
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
            }
            else
            {
                string email = textBoxEmail.Text;
                string caretakers = (string)localSettings.Values["pcaretakers"];
                if (caretakers.Contains(email))
                {
                    errormessage.Text = "this caretaker already exists in your list";
                }
                else
                {
                    //Create an HTTP client object
                    HttpClient httpClient = new HttpClient();

                    Uri requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/AddCaretakerToPatient?code=fYepUksVNfGAVbBZsI5r7b6jrB19nc0bi9p3aF72VqzN56g3kxZbmA==");

                    HttpStringContent stringContent = new HttpStringContent(
                        "{ \"name\": \"" + localSettings.Values["pname"] + "\", " +
                        "\"last\": \"" + localSettings.Values["plast"] + "\"," +
                        "\"id\": \"" + localSettings.Values["pid"] + "\"," +
                        "\"age\": \"" + localSettings.Values["page"] + "\"," +
                        "\"partitionKey\": \"Patients\"," +
                        "\"rowKey\": \"" + localSettings.Values["pemail"] + "\"," +
                        "\"email\": \"" + localSettings.Values["pemail"] + "\"," +
                        "\"password\": \"" + localSettings.Values["ppassword"] + "\"," +
                        "\"caretakers\": \"" + localSettings.Values["pcaretakers"] + " " + email + "\"," +
                          "\"phone\": \"" + localSettings.Values["pphone"] + "\"}",
                        UnicodeEncoding.Utf8, "application/json");

                    try
                    {
                        HttpResponseMessage response = await httpClient.PostAsync(requestUri, stringContent);
                        if ((int)response.StatusCode == 204)
                        {
                            localSettings.Values["pcaretakers"] += " " + email;
                            errormessage.Text = string.Format("added: {0}", email);
                        }
                        else
                        {
                            errormessage.Text = await response.Content.ReadAsStringAsync();
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
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProfilePage));
        }
    }
}
