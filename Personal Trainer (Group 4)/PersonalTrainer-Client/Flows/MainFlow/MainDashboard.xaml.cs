using Microsoft.WindowsAzure.MobileServices;
using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.Flows.Workout;
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

namespace PersonalTrainer_Client.Flows.MainFlow
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainDashboard : Page
    {       
        public static User CurrentUser { get; private set; }
        public static UserTrainingProgram currentTrainingProgram { get; set; }

        public MainDashboard()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        private void StartWorkout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChooseWorkout), currentTrainingProgram);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Frame.BackStack.Clear();

            var parameters = (MainDashboardParams)e.Parameter;
            CurrentUser = parameters.User;
            currentTrainingProgram = await App.UserTrainingProgramsTable.LookupAsync(CurrentUser.CurrentTrainingProgramId);
            StartWorkout.IsEnabled = true;
            Check.IsEnabled = true;
            Statistics.IsEnabled = true;
            Title.Text = "Welcome back, " + CurrentUser.Name + "!";
        }


        private void Check_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TrainingProgramDetails.TrainingProgramDetails), currentTrainingProgram);

        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ChooseWorkoutForStatistics), currentTrainingProgram);

        }
    }
}
