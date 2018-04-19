using PersonalTrainer_Client.DataObjects.Utils;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PersonalTrainer_Client.Flows.Workout.Stats.WorkoutStatistics.xaml
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OtherUsersStatistics : Page
    {
        public OtherUsersStatistics()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var workoutId = (string)e.Parameter;

            var workoutStatistics = App.UserWorkoutStatistics.FindAll(stat => stat.WorkoutId.Equals(workoutId));

            Time.Text += Utils.CalculatePrevWorkoutsPerformanceTimeAverage(workoutStatistics).ToString().Replace('.', ':');
            NumOfEx.Text += Utils.CalculatePrevWorkoutsExNumAverage(workoutStatistics).ToString();

        }

    }
}
