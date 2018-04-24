using Microsoft.Band;
using Microsoft.Band.Sensors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Devices.Geolocation;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Patient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MonitorPage : Page
    {
        String bterr = " Bluetooth signal failure,\n get closer to the band and \n restart monitoring";
         
        public MonitorPage()
        {
            InitializeComponent();
            DataContext = this;
            RunButton.Content = "Run";
            MainButton.Content = "Go To Main";
            AccText.Text = "SVM: ";
            GyroText.Text = "X: \nY: \nZ: ";
            DisText.Text = "Motion: \nPace: \nSpeed: \nTotal: ";
           
            Set_Band();
        }

        /////////////////
        private Geolocator _geolocator = null;
        private double prevLat = 0;
        private double prevLon = 0;
    
        ///////////////
        
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        HttpClient httpClient = new HttpClient();
        Uri requestUri = new Uri("https://firsthellojunk.azurewebsites.net/api/UpdateHRRead?code=nqbeByepqCm7OeZnm1VKZ7eWB95SBdRwts3ULiFi8jYE39y0QWSOSA==");
        Uri requestUriGPS = new Uri(" https://firsthellojunk.azurewebsites.net/api/UpdateGPSReads?code=azGr9/xEdA4XJupPJI/8Lk7YADrp5jCPUh4hvaEa7T/Bru7umuuQpQ==");
        private IBandClient bandClient = null;
        private int samplesReceivedGyro = 0; // the number of Gyroscope samples received
        private int samplesRecievedHR = 0; // the number of HeartRate samples recieved
       
    
               
 
            
        private async void Set_Band()
        {
            RunButton.IsEnabled = false;
            errormessage.Text = "Setting band...";
            IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
            if (pairedBands.Length < 1)
            {
                errormessage.Text = "Band not found";
                return;
            }
            try
            {
                // Connect to Microsoft Band.
                if (bandClient == null)
                {
                    bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);
                }
                if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                }

                // Subscribe to Accelerometer data. 
                bandClient.SensorManager.Accelerometer.ReadingChanged += AccReceived;

                // Subscribe to Calories data. 
                bandClient.SensorManager.Calories.ReadingChanged += CalReceived;

                // Subscribe to Contact data. 
                bandClient.SensorManager.Contact.ReadingChanged += ConReceived;

                // Subscribe to Distance data. 
                bandClient.SensorManager.Distance.ReadingChanged += DistReceived;

                // Subscribe to Gyroscope data. 
                bandClient.SensorManager.Gyroscope.ReadingChanged += GyroReceived;

                // Subscribe to HeartRate data.
                bandClient.SensorManager.HeartRate.ReadingChanged += HeartRateReceived;

                // Subscribe to Pedometer data. 
                bandClient.SensorManager.Pedometer.ReadingChanged += PedoReceived;

                // Subscribe to SkinTemperature data.
                bandClient.SensorManager.SkinTemperature.ReadingChanged += SkinTempReceived;
                errormessage.Text = "Ready to go";
                await bandClient.NotificationManager.VibrateAsync(Microsoft.Band.Notifications.VibrationType.NotificationAlarm);
            }
            catch (Exception ex)
            {
                errormessage.Text = bterr;
            }
            RunButton.IsEnabled = true;
        }

        
    



        
        
        private async void Stop_Band()
        {
            try
            {
                //Stop the sensor subscriptions
                await bandClient.SensorManager.Accelerometer.StopReadingsAsync();
                await bandClient.SensorManager.Calories.StopReadingsAsync();
                await bandClient.SensorManager.Contact.StopReadingsAsync();
                await bandClient.SensorManager.Distance.StopReadingsAsync();
                await bandClient.SensorManager.Gyroscope.StopReadingsAsync();
                await bandClient.SensorManager.HeartRate.StopReadingsAsync();
                await bandClient.SensorManager.Pedometer.StopReadingsAsync();
                await bandClient.SensorManager.SkinTemperature.StopReadingsAsync();

            }
            catch
            {
                return;
            }
        }
       
        private async void Run_Click(object sender, RoutedEventArgs e)
        {
            if (RunButton.Content.Equals("Stop"))
            {
                RunButton.Content = "Run";
                MainButton.IsEnabled = true;
                errormessage.Text = "Ready to go";
               
                Stop_Band();
                
                ////////////////////////
                StopTracking(sender, e);
                /////////////////////
                return;
            }
            try
            {
                
                var now = DateTime.Now.ToString("dd/MM/yyyy@H:mm:ss");
                HttpStringContent stringContent = new HttpStringContent(
                    "{ \"partitionKey\": \"HRs\"," +
                    "\"rowKey\": \"" + localSettings.Values["pemail"] + "\"," +
                    "\"reads\": \"" + localSettings.Values["preads"] +
                 //  "\"," + "\"lastHR\": \"" + now +
                 "\"}",
                    UnicodeEncoding.Utf8, "application/json");




                
                    var now2 = DateTime.Now.ToString("dd/MM/yyyy@H:mm:ss");
                    HttpStringContent stringContent2 = new HttpStringContent(
                        "{ \"partitionKey\": \"Locations\"," +
                        "\"rowKey\": \"" + localSettings.Values["pemail"] + "\"," +
                        "\"readLatitude\": \"" + localSettings.Values["pLatitude"] + "\"," +
                        "\"readLongitude\": \"" + localSettings.Values["pLongitude"] + "\"," +
                        "\"lastLocatoin\": \"" + now2 + "\"}",
                        UnicodeEncoding.Utf8, "application/json");


                
                /*
                catch (Exception ex)
                {
                    errormessage.Text = "Location is unavailble";
                }
                */
                try
                {
                    HttpResponseMessage response = await httpClient.PostAsync(requestUri, stringContent);
                    if ((int)response.StatusCode == 204)
                    {
                        localSettings.Values["plasthr"] = now;
                        
                    }
                    else
                    {
                        errormessage.Text = await response.Content.ReadAsStringAsync();
                    }



                    

                    HttpResponseMessage response2 = await httpClient.PostAsync(requestUriGPS, stringContent2);
                    if ((int)response2.StatusCode == 204)
                    {
                        localSettings.Values["plastLocation"] = now;

                    }
                    else
                    {
                        errormessage.Text = await response2.Content.ReadAsStringAsync();
                    }


                    
                }
                catch (Exception ex)
                {
                    errormessage.Text = "Error: " + ex.Message;
                }
                RunButton.Content = "Stop";
                MainButton.IsEnabled = false;
                errormessage.Text = "Sampling in progress...";

                // Start reading data 
                await bandClient.SensorManager.Accelerometer.StartReadingsAsync();
                await bandClient.SensorManager.Calories.StartReadingsAsync();
                await bandClient.SensorManager.Contact.StartReadingsAsync();
                await bandClient.SensorManager.Distance.StartReadingsAsync();
                await bandClient.SensorManager.Gyroscope.StartReadingsAsync();
                await bandClient.SensorManager.HeartRate.StartReadingsAsync();
                await bandClient.SensorManager.Pedometer.StartReadingsAsync();
                await bandClient.SensorManager.SkinTemperature.StartReadingsAsync();
                
              
                
                if (await Geolocator.RequestAccessAsync() != GeolocationAccessStatus.Allowed)
                {
                    if (await Geolocator.RequestAccessAsync() == GeolocationAccessStatus.Denied)
                    {
                        System.Diagnostics.Debug.Write("Access to location is denied");
                    }
                    else
                   {
                        System.Diagnostics.Debug.Write("Access to location isn't allowed: Unspecified ");
                    }
                 
                    return;
                }
                else
                {
                    await StartTracking(sender, e);

                   


                }
                
                


            
            }
            catch (Exception ex)
            {
                errormessage.Text = bterr;
            }
        }
   

        private async Task<bool> StartTracking(object sender, RoutedEventArgs e)
        {
            // Request permission to access location
            var accessStatus = await Geolocator.RequestAccessAsync();
        
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    // You should set MovementThreshold for distance-based tracking
                    // or ReportInterval for periodic-based tracking before adding event
                    // handlers. If none is set, a ReportInterval of 1 second is used
                    // as a default and a position will be returned every 1 second.
                    //
                    // Value of 2000 milliseconds (2 seconds) 
                    // isn't a requirement, it is just an example.
                 
                    _geolocator = new Geolocator { ReportInterval = 2000 };

                    // Subscribe to PositionChanged event to get updated tracking positions
                    _geolocator.PositionChanged += OnPositionChanged;

                    // Subscribe to StatusChanged event to get updates of location status changes
                    _geolocator.StatusChanged += OnStatusChanged;

                    break;

                case GeolocationAccessStatus.Denied:
                    break;

                case GeolocationAccessStatus.Unspecified:
                    break;
            }
            return true;
        }
        private void StopTracking(object sender, RoutedEventArgs e)
        {
            _geolocator.PositionChanged -= OnPositionChanged;
            _geolocator.StatusChanged -= OnStatusChanged;
            _geolocator = null;

        }

        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                System.Diagnostics.Debug.Write("Latitude " + e.Position.Coordinate.Point.Position.Latitude.ToString());
                System.Diagnostics.Debug.Write("Longitude " + e.Position.Coordinate.Point.Position.Longitude.ToString());
               
                UpdateLocationData(e.Position);
            });
        }

        async private void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Show the location setting message only if status is disabled.

                switch (e.Status)
                {
                    case PositionStatus.Ready:
                        // Location platform is providing valid data.

                        break;

                    case PositionStatus.Initializing:
                        // Location platform is attempting to acquire a fix. 

                        break;

                    case PositionStatus.NoData:
                        // Location platform could not obtain location data.
                        
                        System.Diagnostics.Debug.Write("NoData");
                        break;

                    case PositionStatus.Disabled:
                        // The permission to access location data is denied by the user or other policies.
                     
                        System.Diagnostics.Debug.Write("Disabled");

                        // Show message to the user to go to location settings

                        // Clear cached location data if any
                        UpdateLocationData(null);
                        break;

                    case PositionStatus.NotInitialized:
                        // The location platform is not initialized. This indicates that the application 
                        // has not made a request for location data.
                        System.Diagnostics.Debug.Write("NotInitialized");
                        break;

                    case PositionStatus.NotAvailable:
                        // The location platform is not available on this version of the OS.
                    
                        System.Diagnostics.Debug.Write("NotAvailable");
                        break;

                }
            });
        }
        private void UpdateLocationData(Geoposition position)
        {
            if (position == null)
            {
            }
            else
            {
                double lat = position.Coordinate.Point.Position.Latitude;
                double lon = position.Coordinate.Point.Position.Longitude;
          
              //  errormessage.Text ="you are here"+ position.Coordinate.Point.Position.Latitude.ToString();
              //  errormessage.Text = "you are here" + position.Coordinate.Point.Position.Longitude.ToString();
                System.Diagnostics.Debug.Write("Latitude in Update" + position.Coordinate.Point.Position.Latitude.ToString());
                System.Diagnostics.Debug.Write("Longitude in Update" + position.Coordinate.Point.Position.Longitude.ToString());
                UpdateDBGPS(lat.ToString(), lon.ToString());
              



                if (prevLat == 0)
                {
                    prevLat = lat;
                    prevLon = lon;
                }
         
                prevLat = lat;
                prevLon = lon;
            }
        }
        
       
        
        private async void AccReceived(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> e)
        { 
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                double _x = e.SensorReading.AccelerationX;
                double _y = e.SensorReading.AccelerationY;
                double _z = e.SensorReading.AccelerationZ;
                double _svm = Math.Sqrt(Math.Pow(_x, 2) + Math.Pow(_y, 2) + Math.Pow(_z, 2));
                string svm = _svm.ToString();
                AccText.Text = "SVM: " + svm;
                if (_svm > 11)
                {
                    Stop_Band();

                    Frame.Navigate(typeof(FallPage));
                }
            });
        }

        private async void CalReceived(object sender, BandSensorReadingEventArgs<IBandCaloriesReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CalText.Text = e.SensorReading.Calories.ToString();
            });
        }

        private async void ConReceived(object sender, BandSensorReadingEventArgs<IBandContactReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ConText.Text = e.SensorReading.State.ToString();
            });
        }

        private async void DistReceived(object sender, BandSensorReadingEventArgs<IBandDistanceReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                DisText.Text = "Motion: " + e.SensorReading.CurrentMotion.ToString() + "\nPace: " + e.SensorReading.Pace.ToString() + "\nSpeed: " + e.SensorReading.Speed.ToString() + "\nTotal: " + e.SensorReading.TotalDistance.ToString();
            });
        }

        private async void GyroReceived(object sender, BandSensorReadingEventArgs<IBandGyroscopeReading> e)
        {
            samplesReceivedGyro++;
            if ((samplesReceivedGyro % 20) == 0)
            {
                // Only report occasional Gyroscope data 
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    GyroText.Text = "X: " + e.SensorReading.AccelerationX.ToString() + "\nY: " + e.SensorReading.AccelerationY.ToString() + "\nZ: " + e.SensorReading.AccelerationZ.ToString();
                });
            }
        }

        private async void HeartRateReceived(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                samplesRecievedHR++;
                int HR = e.SensorReading.HeartRate;
                HRText.Text = HR.ToString();
                if(samplesRecievedHR % 10 == 0)
                {
                    UpdateDB(HRText.Text);
                }
                if (HR > 120) { HRText.Text += " Very Elevated"; HRText.Foreground = new SolidColorBrush(Windows.UI.Colors.Red); }
                else if (HR > 100) { HRText.Text += " Elevated"; HRText.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkOrange); }
                else if (HR < 40) { HRText.Text += " Very Low"; HRText.Foreground = new SolidColorBrush(Windows.UI.Colors.Red); }
                else if (HR < 60) { HRText.Text += " Low"; HRText.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkOrange); }
                else { HRText.Text += " Normal"; HRText.Foreground = new SolidColorBrush(Windows.UI.Colors.LawnGreen); }
            });
        }

        private async void SkinTempReceived(object sender, BandSensorReadingEventArgs<IBandSkinTemperatureReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                STText.Text = e.SensorReading.Temperature.ToString();
            });
        }

        private async void PedoReceived(object sender, BandSensorReadingEventArgs<IBandPedometerReading> e)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
            PDText.Text = e.SensorReading.TotalSteps.ToString();
            });
        }
         
        private async void UpdateDB(string HR)
        {
            var now = DateTime.Now.ToString("dd/MM/yyyy@H:mm:ss");
            var newReads = ((string)localSettings.Values["preads"]).Trim().Split(' ').ToList();
            if(newReads.Count() == 500)
            {
                newReads.RemoveAt(0);
            }
            newReads.Add(HR);
            var result = String.Join(" ", newReads.ToArray());
            HttpStringContent stringContent = new HttpStringContent(
                "{ \"partitionKey\": \"HRs\"," +
                "\"rowKey\": \"" + localSettings.Values["pemail"] + "\"," +
                "\"reads\": \"" + result + "\"," +
                "\"lastHR\": \"" + now + "\"}",
                UnicodeEncoding.Utf8, "application/json");

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(requestUri, stringContent);
                if ((int)response.StatusCode == 204)
                {
                    localSettings.Values["plasthr"] = now;
                    localSettings.Values["preads"] = result;
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
                { errormessage.Text = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message; }
            }
        }
        ///////////////////////////////////////////////////////////////////

     

        private async void UpdateDBGPS(string latitude, string longitude )
        {
            var now2 = DateTime.Now.ToString("dd/MM/yyyy@H:mm:ss");
            var newReadsLat = ((string)localSettings.Values["pLatitude"]).Trim().Split(' ').ToList();
            var newReadsLon = ((string)localSettings.Values["pLongitude"]).Trim().Split(' ').ToList();
            if (newReadsLat.Count() == 500)
            {
                newReadsLat.RemoveAt(0);
            }
            newReadsLat.Add(latitude);
            var resultLat = String.Join(" ", newReadsLat.ToArray());

            if (newReadsLon.Count() == 500)
            {
                newReadsLon.RemoveAt(0);
            }
            newReadsLon.Add(longitude);
            var resultLon = String.Join(" ", newReadsLon.ToArray());

            
            HttpStringContent stringContent2 = new HttpStringContent(
                "{ \"partitionKey\": \"Locations\"," +
                "\"rowKey\": \"" + localSettings.Values["pemail"] + "\"," +
                "\"readsLatitude\": \"" + resultLat + "\"," +
                 "\"readsLongitude\": \"" + resultLon +
               // "\"," + "\"lastLocatoin\": \"" + now2 + 
                "\"}",
                UnicodeEncoding.Utf8, "application/json");

            try
            {
                HttpResponseMessage response2 = await httpClient.PostAsync(requestUriGPS, stringContent2);
                if ((int)response2.StatusCode == 204)
                {
                 //   localSettings.Values["plastLocation"] = now;
                    localSettings.Values["pLatitude"] = resultLat;
                    localSettings.Values["pLongitude"] = resultLon;
                }
                else
                {
                    errormessage.Text = await response2.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
              
                { errormessage.Text = " Data Location is unavailable " +  ex.Message; }
            }
        }










        //////////////////////////////////////////////////////////////
        

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (bandClient != null)
            {
                bandClient.Dispose();
            }
            bandClient = null;
        }


      


        private void Main_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
       
    }
}
