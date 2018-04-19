using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Media.SpeechSynthesis;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.Flows.Workout;
using Windows.UI.Core;

namespace PersonalTrainer_Client
{

    public sealed partial class Set : Page
    {

        AccelerometerModel _accelerometerModel = new AccelerometerModel();
        HeartRateModel _hearRateModel = new HeartRateModel();
        PeakDetProcessor _peakDetProcessor = new PeakDetProcessor(0.3);
        double _repsCount = 0;
        SpeechSynthesizer synth;
        DefaultExercise exercise;
        DateTimeOffset startTime;
        DispatcherTimer dispatcherTimer;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            exercise = SetParams.exercise;
            base.OnNavigatedTo(e);         
            StartComponnents();          
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _accelerometerModel.Changed -= _accelerometerModel_Changed;
            _accelerometerModel.Stop();
            _hearRateModel.Changed -= _heartRaterModel_Changed;
            _hearRateModel.Stop();
            dispatcherTimer.Stop();
            _repsCount = 0;
            media.Stop();
            try
            {
                synth.Dispose();
            }
            catch (Exception)
            {

            }

            ExerciseParams.isSetFinished = true;
        }

        public Set()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            this.InitializeComponent();
            
        }

        private async void StartComponnents()
        {

            // Time
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            startTime = DateTimeOffset.Now;
            dispatcherTimer.Start();

            // Reps
            _accelerometerModel.Init();
            _accelerometerModel.Changed += _accelerometerModel_Changed;
            _accelerometerModel.Start();
            reps.Text = "" + _repsCount + " / " + exercise.IntermediateReps;
            
            // Heart Rate
            await _hearRateModel.InitAsync();
            _hearRateModel.Changed += _heartRaterModel_Changed;
            _hearRateModel.Start();

            // Speak
            synth = new SpeechSynthesizer();
            Speak("Let's go!");

            End.IsEnabled = true;
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            TimeSpan span = now - startTime;
            time.Text = span.ToString(@"mm\:ss");
        }

        void _accelerometerModel_Changed(long timestamp, double force)
        {
            if(_peakDetProcessor.ProcessPoint(timestamp, force)){
                _repsCount += 0.5;
                if(_repsCount % 1 == 0)
                {
                    reps.Text = "" + _repsCount + " / " + exercise.IntermediateReps;
                }

                if(exercise.IntermediateReps - _repsCount == 3) Speak("Last 3");
                else if(exercise.IntermediateReps - _repsCount == 2) Speak("2");
                else if(exercise.IntermediateReps - _repsCount == 1) Speak("1");
                else if(exercise.IntermediateReps - _repsCount == 0) Speak("Great job");
                else if (_repsCount % 5 == 0) Speak("" + _repsCount);
            }
        }

        void _heartRaterModel_Changed(int heart_rate, double accuracy)
        {
            if(accuracy < 1)
            {
                heartRate.Text = "-";
            }
            else
            {
                heartRate.Text = "" + heart_rate;
            }
            
            heartRateImage.Opacity = accuracy;
        }

        async void Speak(string message)
        {
            var stream = await synth.SynthesizeTextToStreamAsync(message);
            media.SetSource(stream, stream.ContentType);
            media.Play();
        }

        private void Finish(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

    }

    public class PeakDetProcessor
    {

        double minValue = double.MinValue;
        double maxValue = double.MaxValue;
        long minTimestamp = 0;
        long maxTimestamp = 0;

        bool lookForMax = true;
        private double delta;

        public PeakDetProcessor(double delta)
        {
            this.delta = delta;
        }

        public bool ProcessPoint(long timestamp, double magnitude)
        {

            if (magnitude > maxValue)
            {
                maxValue = magnitude;
                maxTimestamp = timestamp;
            }
            if (magnitude < minValue)
            {
                minValue = magnitude;
                minTimestamp = timestamp;
            }

            if (lookForMax)
            {
                if (magnitude < maxValue - delta)
                {
                    minValue = magnitude;
                    minTimestamp = timestamp;
                    lookForMax = false;

                    return false;
                }
            }
            else
            {
                if (magnitude > minValue + delta)
                {
                    maxValue = magnitude;
                    maxTimestamp = timestamp;
                    lookForMax = true;
                    return true;
                }
            }

            return false;
        }
    }
}
