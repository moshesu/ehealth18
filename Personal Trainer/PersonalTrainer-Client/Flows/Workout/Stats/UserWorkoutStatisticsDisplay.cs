using Personal_TrainerService.DataObjects;
using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.Utils;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.Flows.MainFlow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PersonalTrainer_Client.Flows.Workout.Stats
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserWorkoutStatisticsDisplay : Page
    {
        private DefaultWorkout m_workout;
        private List<UserWorkoutStatistics> m_workoutStatistics;

        public UserWorkoutStatisticsDisplay()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            var workoutId = UserWorkoutStaticsParams.WorkoutId;
            m_workout = await App.DefaultWorkoutsTable.LookupAsync(workoutId);

            if (UserWorkoutStaticsParams.Myself) 
            {
                Title.Text = "Previous Workouts Statistics";
                m_workoutStatistics = App.UserWorkoutStatistics.FindAll(stat => stat.UserId.Equals(MainDashboard.CurrentUser.Id) && stat.WorkoutId.Equals(workoutId));
                m_workoutStatistics.Sort(delegate (UserWorkoutStatistics x, UserWorkoutStatistics y)
                {
                    return (int)(x.PerformanceNumberCount - y.PerformanceNumberCount);
                });
            }
            else
            {
                Title.Text = "Other Trainers Statistics";
                m_workoutStatistics = App.UserWorkoutStatistics.FindAll(stat => !stat.UserId.Equals(MainDashboard.CurrentUser.Id) && stat.WorkoutId.Equals(workoutId));
                m_workoutStatistics.Sort(delegate (UserWorkoutStatistics x, UserWorkoutStatistics y)
                {
                    return (int)(x.PerformanceNumberCount - y.PerformanceNumberCount);
                });
                int max = m_workoutStatistics.ElementAt(m_workoutStatistics.Count - 1).PerformanceNumberCount;
                List<UserWorkoutStatistics> averageStatistics = new List<UserWorkoutStatistics>();
                
                for(int i = 1; i <= max; i++)
                {
                    List<UserWorkoutStatistics> statisticsForIndex = m_workoutStatistics.FindAll(stat => stat.PerformanceNumberCount == i);
                    if (statisticsForIndex.Count() != 0)
                    {
                        UserWorkoutStatistics averageStatisticsForIndex = new UserWorkoutStatistics();
                        averageStatisticsForIndex.PerformanceTimeAvarage = Utils.CalculatePrevWorkoutsPerformanceTimeAverage(statisticsForIndex);
                        averageStatisticsForIndex.GeneralColumn1 = Utils.CalculatePrevWorkoutsExNumAverage(statisticsForIndex);
                        averageStatisticsForIndex.HeartReatAvarage = Utils.CalculatePrevWorkoutsAverageHeartRateAverage(statisticsForIndex);
                        averageStatisticsForIndex.MaximalHeartRateAvarage = Utils.CalculatePrevWorkoutsPeakHeartRateAverage(statisticsForIndex);
                        averageStatisticsForIndex.CaloriesBurnedAvarage = Utils.CalculatePrevWorkoutsCaloriesAverage(statisticsForIndex);
                        averageStatistics.Add(averageStatisticsForIndex);
                    }
                }
                
                m_workoutStatistics = averageStatistics;
            }

            if(m_workoutStatistics.Count == 0)
            {
                var dialog = new MessageDialog("No statistics found");
                dialog.Commands.Add(new UICommand("Return"));
                await dialog.ShowAsync();
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.GoBack();
            }
            else
            {
                TimeSpan span = TimeSpan.FromMinutes(Utils.CalculatePrevWorkoutsPerformanceTimeAverage(m_workoutStatistics));

                Time.Text += span.ToString(@"hh\:mm");
                NumOfEx.Text += Utils.CalculatePrevWorkoutsExNumAverage(m_workoutStatistics).ToString() + "/" + (m_workout.ExercisesIds.Split(';').Count());
                heartRate.Text += Utils.CalculatePrevWorkoutsAverageHeartRateAverage(m_workoutStatistics);
                peak_heart_rate.Text += Utils.CalculatePrevWorkoutsPeakHeartRateAverage(m_workoutStatistics);
                calories.Text += Utils.CalculatePrevWorkoutsCaloriesAverage(m_workoutStatistics);

                List<Records> timeRecords = new List<Records>();
                for (int i = 0; i < m_workoutStatistics.Count; i++)
                {
                    timeRecords.Add(new Records() { Name = "" + (i + 1), Amount = (int)m_workoutStatistics.ElementAt(i).PerformanceTimeAvarage });

                }

                List<Records> exercisesAmountRecords = new List<Records>();
                for (int i = 0; i < m_workoutStatistics.Count; i++)
                {
                    exercisesAmountRecords.Add(new Records() { Name = "" + (i + 1), Amount = (int)(m_workoutStatistics.ElementAt(i).GeneralColumn1 / (m_workout.ExercisesIds.Split(';').Count()) * 100) });

                }

                List<Records> heartRatesRecords = new List<Records>();
                for (int i = 0; i < m_workoutStatistics.Count; i++)
                {
                    heartRatesRecords.Add(new Records() { Name = "" + (i + 1), Amount = (int)m_workoutStatistics.ElementAt(i).HeartReatAvarage });

                }

                List<Records> peakHeartRatesRecords = new List<Records>();
                for (int i = 0; i < m_workoutStatistics.Count; i++)
                {
                    peakHeartRatesRecords.Add(new Records() { Name = "" + (i + 1), Amount = (int)m_workoutStatistics.ElementAt(i).MaximalHeartRateAvarage });

                }

            (LineChart.Series[0] as LineSeries).ItemsSource = timeRecords;
                (LineChart.Series[1] as LineSeries).ItemsSource = exercisesAmountRecords;
                (LineChart.Series[2] as LineSeries).ItemsSource = heartRatesRecords;
                (LineChart.Series[3] as LineSeries).ItemsSource = peakHeartRatesRecords;
            }

            

        }

        private void MainDashboardReturn_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Records
    {
        public string Name
        {
            get;
            set;
        }
        public int Amount
        {
            get;
            set;
        }
    }
}
