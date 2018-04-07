using Personal_TrainerService.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.DataObjects.Utils;
using PersonalTrainer_Client.Flows.MainFlow;
using PersonalTrainer_Client.Flows.Workout.Stats;
using PersonalTrainer_Client.Flows.Workout.Stats.WorkoutStatistics.xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PersonalTrainer_Client.Flows.Workout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WorkoutSummary : Page
    {
        private List<UserWorkoutStatistics> m_prevWorkoutsStatistics;

        public WorkoutSummary()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            TimeSpan span = now - App.startTime;
            String timeString = span.ToString(@"hh\:mm");

            Time.Text += timeString;
            NumOfEx.Text += WorkoutSummaryParams.numberOfExercisesPerformed.ToString() + "/" + WorkoutSummaryParams.numberOfExercisesTotal.ToString();
            heartRate.Text += HeartRateModel.getAverage();
            peak_heart_rate.Text += HeartRateModel.getPeak();
            calories.Text += CaloriesModel.calories;

            var prevWorkoutsStatistics = App.UserWorkoutStatistics.FindAll(stat => stat.UserId.Equals(MainDashboard.CurrentUser.Id) && stat.WorkoutId.Equals(WorkoutSummaryParams.WorkoutId));
            var otherUsersStatisticsCount = App.UserWorkoutStatistics.FindAll(stat => stat.WorkoutId.Equals(WorkoutSummaryParams.WorkoutId)).Count;


            m_prevWorkoutsStatistics = prevWorkoutsStatistics;

            if (prevWorkoutsStatistics.Count != 0)
            {
                var overallTime = span.TotalMinutes;

                var prevWorkoutsExNumPerformedAverage = Utils.CalculatePrevWorkoutsExNumAverage(prevWorkoutsStatistics);

                var prevWorkoutsPerformanceTimeAvarage = Utils.CalculatePrevWorkoutsPerformanceTimeAverage(prevWorkoutsStatistics);

                if (WorkoutSummaryParams.numberOfExercisesPerformed > prevWorkoutsExNumPerformedAverage)
                {
                    CompareToStat.Text = "Great job! You completed more exercises than your previous workouts! Keep going!";
                    NumOfEx.Foreground = new SolidColorBrush(Colors.Green);
                    feedback_image.Source = new BitmapImage(new Uri("ms-appx:///Images/gold.png"));
                }
                else if (WorkoutSummaryParams.numberOfExercisesPerformed < prevWorkoutsExNumPerformedAverage)
                {
                    CompareToStat.Text = "You completed less exercises than your previous workouts. You can do better than this. Try pushing a little harder next time!";
                    NumOfEx.Foreground = new SolidColorBrush(Colors.Red);
                    feedback_image.Source = new BitmapImage(new Uri("ms-appx:///Images/bronze.png"));
                }
                else if (prevWorkoutsPerformanceTimeAvarage > overallTime)
                {
                    CompareToStat.Text = "Great job! Your overall workout time has decreased from previous workouts! Keep going!";
                    Time.Foreground = new SolidColorBrush(Colors.Green);
                    feedback_image.Source = new BitmapImage(new Uri("ms-appx:///Images/gold.png"));
                }
                else
                {
                    CompareToStat.Text = "Your overall workout time was higher from previous workouts. You can do better than this. Try pushing A little harder next time!";
                    Time.Foreground = new SolidColorBrush(Colors.Red);
                    feedback_image.Source = new BitmapImage(new Uri("ms-appx:///Images/bronze.png"));
                }
                

                UserStatistics.Visibility = Visibility.Visible;
            }
            else
            {
                CompareToStat.Text = "Great job! You finished this workout for the first time! Train next week and see your improvments.";
                feedback_image.Source = new BitmapImage(new Uri("ms-appx:///Images/gold.png"));
            }

            if(otherUsersStatisticsCount > 0)
            {
                OtherUsersStatistics.Visibility = Visibility.Visible;
            }

            var newWorkoutStatistics = new UserWorkoutStatistics()
            {
                Id = Guid.NewGuid().ToString(),
                WorkoutId = WorkoutSummaryParams.WorkoutId,
                UserId = MainDashboard.CurrentUser.Id,
                PerformanceTimeAvarage = span.TotalMinutes,
                GeneralColumn1 = WorkoutSummaryParams.numberOfExercisesPerformed,
                PerformanceNumberCount = prevWorkoutsStatistics.Count > 0 ? prevWorkoutsStatistics.Max(stat => stat.PerformanceNumberCount) + 1 : 1,
                HeartReatAvarage = HeartRateModel.getAverage(),
                MaximalHeartRateAvarage = HeartRateModel.getPeak(),
                CaloriesBurnedAvarage = CaloriesModel.calories,
            };

            if (App.updateStatistics)
            {
                await App.UserWorkoutStatisticsTable.InsertAsync(newWorkoutStatistics);

                App.UserWorkoutStatistics = await App.UserWorkoutStatisticsTable.ToListAsync();
            }

            

            App.updateStatistics = false;
        }

        private void MainDashboardReturn_Click(object sender, RoutedEventArgs e) 
        {
            Frame.Navigate(typeof(MainDashboard), new MainDashboardParams(MainDashboard.CurrentUser));
        }

        private void UserStatistics_Click(object sender, RoutedEventArgs e)
        {
            UserWorkoutStaticsParams.WorkoutId = WorkoutSummaryParams.WorkoutId;
            UserWorkoutStaticsParams.Myself = true;
            Frame.Navigate(typeof(UserWorkoutStatisticsDisplay), null );
        }

        private void OtherUsersStatistics_Click(object sender, RoutedEventArgs e)
        {
            UserWorkoutStaticsParams.WorkoutId = WorkoutSummaryParams.WorkoutId;
            UserWorkoutStaticsParams.Myself = false;

            Frame.Navigate(typeof(UserWorkoutStatisticsDisplay), null );
        }
    }


}
