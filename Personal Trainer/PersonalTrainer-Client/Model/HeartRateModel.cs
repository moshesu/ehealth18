using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Band.Sensors;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Core;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using System.Linq;
using Microsoft.Band;

namespace PersonalTrainer_Client
{

    public class HeartRateModel : ViewModel
    {
        public delegate void ChangedHandler(int heartRate, double quality);
        public event ChangedHandler Changed;
        public static int peak = 0;
        public static double sum = 0;
        public static int count = 0;
      
        public async Task InitAsync()
        {
            if (BandModel.IsConnected)
            {
                if (BandModel.BandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await BandModel.BandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                }
                BandModel.BandClient.SensorManager.HeartRate.ReadingChanged += HeartRate_ReadingChanged;
            }
        }

        public void Start()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.HeartRate.StartReadingsAsync();
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.HeartRate.StopReadingsAsync();
            }
        }

        void HeartRate_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> e)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     HeartRateSensorReading reading = new HeartRateSensorReading { HeartRate = e.SensorReading.HeartRate, Quality = e.SensorReading.Quality };
                     count++;
                     sum += reading.Value;
                     if(peak < reading.Value)
                     {
                         peak = reading.Value;
                     }
                     if (Changed != null)
                     {
                         Changed(reading.Value, reading.Accuracy);
                     } 
                 });
        }

        public static int getAverage()
        {
            if(count == 0)
            {
                return 0;
            }
            else
            {
                return (int)(sum / count);
            }
            
        }

        public static int getPeak()
        {
            return peak;
        }

    }
}
