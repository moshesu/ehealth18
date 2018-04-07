using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.Flows.MainFlow;
using PersonalTrainer_Client.Flows.TrainingProgramDetails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PersonalTrainer_Client.Flows.Workout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WorkoutDashboard : Page
    {
        DefaultWorkout currentWorkout { get; set; }
        List<string> ExersiceIds { get; set; }
        int m_nextExrciseIndex { get; set; }
        SpeechSynthesizer synth = new SpeechSynthesizer();
        CaloriesModel _caloriesModel = new CaloriesModel();

        public WorkoutDashboard()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        private void StartExercise_Click(object sender, RoutedEventArgs e)
        {

            ExerciseParams.exerciseId = ExersiceIds[m_nextExrciseIndex];
            Frame.Navigate(typeof(Exercise), null);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Finish.IsEnabled = true;
            WorkoutDetails.IsEnabled = true;

            var workoutId = WorkoutParams.workoutId;

            if (WorkoutParams.resetTimer == true)
            {
                App.workoutTimer = new DispatcherTimer();
                App.workoutTimer.Interval = new TimeSpan(0, 0, 1);
                App.startTime = DateTimeOffset.Now;
                App.workoutTimer.Start();
                CaloriesModel.startCalories = -1;
                CaloriesModel.calories = 0;
                HeartRateModel.count = 0;
                HeartRateModel.sum = 0;
                HeartRateModel.peak = 0;
                WorkoutParams.resetTimer = false;
            }

            App.workoutTimer.Tick += dispatcherTimer_Tick;

            if (WorkoutParams.startNewWorkout || currentWorkout == null)
            {
                currentWorkout = await App.DefaultWorkoutsTable.LookupAsync(workoutId);
                m_nextExrciseIndex = 0;

                WorkoutParams.startNewWorkout = false;
            }
            if(WorkoutParams.isFinishedExercise)
            {
                m_nextExrciseIndex++;
                ExerciseParams.startNewExercise = true;
                WorkoutParams.isFinishedExercise = false;
            }

            ExersiceIds = currentWorkout.ExercisesIds.Split(';').ToList();

            if (m_nextExrciseIndex >= ExersiceIds.Count())
            {
                StartExercise.IsEnabled = false;
                StartExercise.Visibility = Visibility.Collapsed;
                NextEx.Text = "Done";
                NextEx.FontSize = 30;
            }
            else
            {
                StartExercise.IsEnabled = true;
                StartExercise.Visibility = Visibility.Visible;
                NextEx.FontSize = 20;
                var fireAndForget = calculateNextExName(m_nextExrciseIndex);
            }

            // Calories
            await BandModel.InitAsync();
            _caloriesModel.Init();
            _caloriesModel.Changed += _caloriesModel_Changed;
            _caloriesModel.Start();

        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (NextEx.Text.Equals("Done"))
            {
                NextEx.Text = "";
            }
        }


        async void Speak(string message)
        {
            var stream = await synth.SynthesizeTextToStreamAsync(message);
            media.SetSource(stream, stream.ContentType);
            media.Play();
        }

        private async Task calculateNextExName(int nextExIndex)
        {
            var nextEx = await App.DefaultExercisesTable.LookupAsync(ExersiceIds[nextExIndex]);
            NextEx.Text = nextEx.Name;
        }


        private void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            TimeSpan span = now - App.startTime;
            time.Text = span.ToString(@"mm\:ss");
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            _caloriesModel.Changed -= _caloriesModel_Changed;
            _caloriesModel.Stop();
            App.updateStatistics = true;
            WorkoutSummaryParams.numberOfExercisesPerformed = m_nextExrciseIndex;
            WorkoutSummaryParams.numberOfExercisesTotal = ExersiceIds.Count;
            WorkoutSummaryParams.WorkoutId = currentWorkout.Id;

            Frame.Navigate(typeof(WorkoutSummary), null);
        }

        private void WorkoutDetails_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WorkoutDetails), new WorkoutDetailsParams(currentWorkout, MainDashboard.currentTrainingProgram));
        }

        void _caloriesModel_Changed(long cal)
        {
            calories.Text = "" + cal;
        }
    }
}
