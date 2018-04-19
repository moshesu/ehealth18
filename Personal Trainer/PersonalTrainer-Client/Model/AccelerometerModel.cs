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

    public class AccelerometerModel : ViewModel
    {
        public delegate void ChangedHandler(long timestamp, double force);
        public event ChangedHandler Changed;

        DateTimeOffset _startedTime = DateTimeOffset.MinValue;
        AccelerometerSensorReading _last;
        int count = 0;
      
        public void Init()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                BandModel.BandClient.SensorManager.Accelerometer.ReportingInterval = TimeSpan.FromMilliseconds(16.0);
            }
        }

        public void Start()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Accelerometer.StartReadingsAsync(new CancellationToken());
                BandModel.BandClient.NotificationManager.VibrateAsync(Microsoft.Band.Notifications.VibrationType.OneToneHigh);
            }
        }

        public void Stop()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Accelerometer.StopReadingsAsync();
                BandModel.BandClient.SensorManager.Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
            }
        }

        void Accelerometer_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> e)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                    AccelerometerSensorReading reading = new AccelerometerSensorReading { X = e.SensorReading.AccelerationX, Y = e.SensorReading.AccelerationY, Z = e.SensorReading.AccelerationZ };
                    _last = reading;
                    Recalculate();
                 });
        }


        void Recalculate()
        {
           
            count = count % 12;
            if (Changed != null && count == 0)
            {
                Changed(_startedTime.Ticks, _last.Value);
            }
            count++;
                
        }

        
    }
}
