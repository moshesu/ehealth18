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


using Windows.Devices.Geolocation;

using Windows.Services.Maps;
using Windows.Storage.Streams;

using Windows.UI.Xaml.Controls.Maps;
using Windows.Web.Http;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Caretaker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        public MapPage()
        {
            this.InitializeComponent();
        }

        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameter = e.Parameter as string;
            Globals._email = parameter;
            System.Diagnostics.Debug.Write("\n line 55" + Globals._email);
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
                    var readsLatitude = settings[8].Split(' ');
                    var readsLatitudeString = readsLatitude[readsLatitude.Length - 1];
                    var readsLongtitude = settings[9].Split(' ');
                    var readsLongtitudeString = readsLongtitude[readsLongtitude.Length - 1];
                    BasicGeoposition location = new BasicGeoposition();
                    location.Latitude = double.Parse(readsLatitudeString, System.Globalization.CultureInfo.InvariantCulture);
                    location.Longitude = double.Parse(readsLongtitudeString, System.Globalization.CultureInfo.InvariantCulture);
                    Geopoint pointOfPosition = new Geopoint(location);


                    MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointOfPosition);

                    if (result.Status == MapLocationFinderStatus.Success && result.Locations.Count > 0)
                    {

                        System.Diagnostics.Debug.Write(" line 88"+  Globals._email);
                    }
                    else
                    {
                        errormessage.Text = "Not Found";
                        // tbOutputText.Text = "No results";
                        System.Diagnostics.Debug.Write(" No results");
                    }
                   
                }
            }

            catch (Exception ex)
            {
                if (ex.HResult.ToString("X") == "80072EE7")
                {
                    errormessage.Text = "Error: " + " Message: check your internet connection";
                }
                else
                {
                    errormessage.Text = "Error: " + ex.Message;
                }
            }


        } 

   

        private async void Map_Loaded(object sender, RoutedEventArgs e)
        {
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
            ///////////////////////////////////////////////////////////////////////////
            System.Diagnostics.Debug.Write("line 120 " + Globals._email);
            if (PMap.IsEnabled)
            {
                //PMap.Style = MapStyle;
                PMap.Style = MapStyle.Terrain;
               
                PMap.MapServiceToken = apiKey;
                 System.Diagnostics.Debug.Write("line 132 "+Globals._email);
                HttpClient httpClient = new HttpClient();
                Uri requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/FetchPatient?code=LBn9ZSPhn7U5ZsKYaC4ICiP5KX6v/tcpxj855db4Vh6p8lsR2FAHUA==&email=" + Globals._email + "&password=nopw");

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(requestUri);
                    if ((int)response.StatusCode != 200)

                    {
                        System.Diagnostics.Debug.Write("line 136 " + Globals._email);
                        errormessage.Text = "patient info unavailable";
                    }
                    else
                    {
                        var textResponse = await response.Content.ReadAsStringAsync();

                        var settings = textResponse.Trim('"').Split(',');
                        var readsLatitude = settings[8].Split(' ');
                        var readsLatitudeString = readsLatitude[readsLatitude.Length - 1];
                        var readsLongtitude = settings[9].Split(' ');
                        var readsLongtitudeString = readsLongtitude[readsLongtitude.Length - 1];
                        System.Diagnostics.Debug.Write("line 148 " + readsLongtitudeString);
                        BasicGeoposition location = new BasicGeoposition();
                        location.Latitude = double.Parse(readsLatitudeString, System.Globalization.CultureInfo.InvariantCulture);
                        location.Longitude = double.Parse(readsLongtitudeString, System.Globalization.CultureInfo.InvariantCulture);
                        Geopoint pointOfPosition = new Geopoint(location);


                        MapLocationFinderResult result = await MapLocationFinder.FindLocationsAtAsync(pointOfPosition);


                        if (result.Status == MapLocationFinderStatus.Success && result.Locations.Count > 0)
                        {
                            //create POI
                            MapIcon myPOI = new MapIcon { Location = pointOfPosition, Title = "Patient's Location", NormalizedAnchorPoint = new Point(1.5, 1.0),ZIndex = 0 };
                            // Display an image of a MapIcon   
                            myPOI.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/pin.png"));
                            // add to map and center it
                            PMap.MapElements.Add(myPOI);
                            PMap.Center = pointOfPosition;
                            PMap.ZoomLevel = 10;

                            MapScene mapScene = MapScene.CreateFromLocationAndRadius(new Geopoint(location), 500, 150, 70);
                            await PMap.TrySetSceneAsync(mapScene);
                        }
                        else
                        {
                            errormessage.Text = "Not Found";
                        }
                    }
                }
                catch(Exception ex)
                {
                    errormessage.Text = "Location not found";
                }
            }
        }
        private void Info_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(InfoPage), Globals._email);
        }
    }
} 
