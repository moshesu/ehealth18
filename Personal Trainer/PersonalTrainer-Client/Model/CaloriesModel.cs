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

namespace PersonalTrainer_Client
{

    public class CaloriesModel : ViewModel
    {
        public delegate void ChangedHandler(long calories);
        public event ChangedHandler Changed;
        public static long startCalories = -1;
        DateTimeOffset _startedTime = DateTimeOffset.MinValue;
        CaloriesSensorReading _last;
        int count = 0;
        public static int calories;
        public void Init()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Calories.ReadingChanged += Calories_ReadingChanged;
                BandModel.BandClient.SensorManager.Calories.ReportingInterval = TimeSpan.FromMilliseconds(1000);
            }
        }

        public void Start()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Calories.StartReadingsAsync(new CancellationToken());
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Calories.StopReadingsAsync();
            }
        }

        void Calories_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandCaloriesReading> e)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     CaloriesSensorReading reading = new CaloriesSensorReading { Calories = e.SensorReading.Calories};
                    _last = reading;
                     count++;
                     if (Changed != null)
                     {
                         if(startCalories == -1)
                         {
                             startCalories = reading.Calories;
                         }
                         calories = (int)(reading.Calories - startCalories);
                         Changed(reading.Calories - startCalories);
                     }
                 });
        }
    }
}
