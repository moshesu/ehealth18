using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Services.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;


using Windows.Storage.Streams;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Caretaker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ////////////////////////////////////
            errormessage.Text = "";
            string apiKey = "";

            //Create an HTTP client object
            HttpClient httpClient1 = new HttpClient();

            Uri requestUri1 = new Uri("https://funcappformail.azurewebsites.net/api/GPSGetKey?code=DP9x6xuw5LrZ09paLdpRkaIXSu2Ad4sjX3nJL6C1WauW5UikOW7W1g==");

            HttpStringContent stringContent = new HttpStringContent(
                "{ \"key\": \"key\"}",
                UnicodeEncoding.Utf8, "application/json");

            try
            {
                HttpResponseMessage getResponse = await httpClient1.PostAsync(requestUri1, stringContent);

                if ((int)getResponse.StatusCode != 200)
                {
                    errormessage.Text = getResponse.StatusCode.ToString();
                }
                else
                {
                    apiKey = await getResponse.Content.ReadAsStringAsync();
                    apiKey = apiKey.Remove(0, 1);
                    apiKey = apiKey.Remove(apiKey.Length - 1, 1);
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



            ////////////////////////////////////////
            var parameter = e.Parameter as string;
            Globals._email = parameter;
            HttpClient httpClient = new HttpClient();

            Uri requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/FetchPatient?code=LBn9ZSPhn7U5ZsKYaC4ICiP5KX6v/tcpxj855db4Vh6p8lsR2FAHUA==&email=" + Globals._email + "&password=nopw");
 
            try
            { 
                HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                if ((int)response.StatusCode != 200)
                {
                    errormessage.Text = "patient info unavailable";
                }
                else
                {
                    var textResponse = await response.Content.ReadAsStringAsync();
                   
                    var settings = textResponse.Trim('"').Split(',');
                    textBlockEmail.Text = Globals._email;
                    textBlockFirstName.Text = settings[0];
                    textBlockLastName.Text = settings[1];
                    textBlockID.Text = settings[2];
                    textBlockAge.Text = settings[3];
                    textBlockPhone.Text = settings[4];
                    textBlockLO.Text = settings[6];
                    
                    var reads = settings[7].Split(' ');
                    
                    textBlockHR.Text = reads[reads.Length - 1];

                    var readsLatitude = settings[8].Split(' ');
                    var readsLatitudeString = readsLatitude[readsLatitude.Length - 1];
                    var readsLongtitude = settings[9].Split(' ');
                    var readsLongtitudeString = readsLongtitude[readsLongtitude.Length - 1];

                    

                    AddColor();
                    errormessage.Text = "Viewing [" + settings[0] + " " + settings[1] + "]'s information";




                    ///////////////////
                    // The location to reverse geocode.


                   
                    MapService.ServiceToken = apiKey;
                     // var apiKey = Environment.GetEnvironmentVariable("mapMyCare2");
                     // MapService.ServiceToken = apiKey;

                     BasicGeoposition location = new BasicGeoposition();
                    location.Latitude =  double.Parse(readsLatitudeString, System.Globalization.CultureInfo.InvariantCulture);
                    location.Longitude = double.Parse(readsLongtitudeString, System.Globalization.CultureInfo.InvariantCulture);
                    Geopoint pointToReverseGeocode = new Geopoint(location);

                    // Reverse geocode the specified geographic location.
                    MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

                
                    if (result.Status == MapLocationFinderStatus.Success && result.Locations.Count > 0)
                    {
                        

                        var StreetNumber = result.Locations[0].Address.StreetNumber;
                        if (StreetNumber == "")
                        {
                            StreetNumber = "Unknown";
                        }
                        textBlockGPS.Text = "country: " + result.Locations[0].Address.Country + "\n" + "town: " + result.Locations[0].Address.Town + "\n" + "street: " + result.Locations[0].Address.Street + "\n" + "street Number: " + StreetNumber;


                        System.Diagnostics.Debug.Write("country: " + result.Locations[0].Address.Country + "\n" + "town: " + result.Locations[0].Address.Town + "\n" + "street: " + result.Locations[0].Address.Street + "\n" + "street Number: " + StreetNumber);
                        
                    }
                    else if (result.Locations.Count == 0)
                    {
                        textBlockGPS.Text = " Not Found";
                       
                       System.Diagnostics.Debug.Write( " No results");
                    }
                    else
                    {
                        textBlockGPS.Text = " Not Found";
                       
                        System.Diagnostics.Debug.Write("Status " + result.Status.ToString());

                    }
                   


                }
            }
            catch (Exception ex)
            {
                if (ex.HResult.ToString("X") == "80072EE7")
                {
                    errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: check your internet connection";
                }
                if (ex.HResult.ToString("X") == "80131537")
                {
                    // for case of !200 already checked, getting here save
                    HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                    var textResponse = await response.Content.ReadAsStringAsync();

                    var settings = textResponse.Trim('"').Split(',');
                    textBlockEmail.Text = Globals._email;
                    textBlockFirstName.Text = settings[0];
                    textBlockLastName.Text = settings[1];
                    errormessage.Text = "Viewing [" + settings[0] + " " + settings[1] + "]'s information";
                }
                else
                {
                    errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                }
            }
        }
              
            private void AddColor()
        {
            try
            {
                int HR = Int32.Parse(textBlockHR.Text);
                if (HR > 120) { textBlockHR.Text += " Very Elevated"; textBlockHR.Foreground = new SolidColorBrush(Windows.UI.Colors.Red); }
                else if (HR > 100) { textBlockHR.Text += " Elevated"; textBlockHR.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkOrange); }
                else if (HR < 40) { textBlockHR.Text += " Very Low"; textBlockHR.Foreground = new SolidColorBrush(Windows.UI.Colors.Red); }
                else if (HR < 60) { textBlockHR.Text += " Low"; textBlockHR.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkOrange); }
                else { textBlockHR.Text += " Normal"; textBlockHR.Foreground = new SolidColorBrush(Windows.UI.Colors.LawnGreen); }
            }
            catch { }
        }

        private void List_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ListPage));
        }

        private void Refresh_Click(object sender, RoutedEventArgs e) 
        {
            this.Frame.Navigate(typeof(InfoPage), Globals._email);
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapPage),Globals._email);
        }

        
    }
}
