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
    public sealed partial class AddPatientPage : Page
    {
        public AddPatientPage() 
        {
            InitializeComponent();
        }

        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            errormessage.Text = "";
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
                string patients = (string)localSettings.Values["cpatients"];
                if (patients.Contains(email))
                {
                    errormessage.Text = "this patient already exists in your list";
                }
                else
                {
                    //Create an HTTP client object
                    HttpClient httpClient = new HttpClient();

                    Uri requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/FetchPatient?code=LBn9ZSPhn7U5ZsKYaC4ICiP5KX6v/tcpxj855db4Vh6p8lsR2FAHUA==&email=" + email + "&password=nopw");

                    try
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                        if ((int)response.StatusCode != 200)
                        {
                            errormessage.Text = "Incorrect email or patient doesnt exist";
                            return;
                        }
                        else
                        {
                            var textResponse = await response.Content.ReadAsStringAsync();
                            var settings = textResponse.Trim('"').Split(',');
                            if (!settings[5].Contains((string)localSettings.Values["cemail"]))
                            {
                                errormessage.Text = "This patient didnt add you as a caretaker. Please contact the patient.";
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                    }

                    requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/AddPatientToCaretaker?code=jlIqQ/AKmMjN0Adf0za1cGrQ2YzMNlbJOlBt2V0uHG7VSXuFqPXYTg==");

                    HttpStringContent stringContent = new HttpStringContent(
                        "{ \"name\": \"" + localSettings.Values["cname"] + "\", " +
                        "\"last\": \"" + localSettings.Values["clast"] + "\"," +
                        "\"partitionKey\": \"Caretakers\"," +
                        "\"rowKey\": \"" + localSettings.Values["cemail"] + "\"," +
                        "\"email\": \"" + localSettings.Values["cemail"] + "\"," +
                        "\"password\": \"" + localSettings.Values["cpassword"] + "\"," +
                        "\"patients\": \"" + localSettings.Values["cpatients"] + " " + email + "\"}",
                        UnicodeEncoding.Utf8, "application/json");

                    try
                    {
                        HttpResponseMessage response = await httpClient.PostAsync(requestUri, stringContent);
                        if ((int)response.StatusCode == 204)
                        {
                            localSettings.Values["cpatients"] += " " + email;
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

        private void List_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ListPage));
        }
    }
}
