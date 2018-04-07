using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.Flows.TrainingProgramDetails;
using PersonalTrainer_Client.Flows.Workout;
using PersonalTrainer_Client.Flows.Workout.Stats;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PersonalTrainer_Client.Flows.MainFlow
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChooseWorkoutForStatistics : Page
    {
        private UserTrainingProgram trainingProgram;
        SpeechSynthesizer synth = new SpeechSynthesizer();
        CaloriesModel _caloriesModel = new CaloriesModel();

        public ChooseWorkoutForStatistics()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            trainingProgram = (UserTrainingProgram)e.Parameter;

            ObservableCollection<string> listItems = new ObservableCollection<string>();

            if (trainingProgram.SundayWorkoutId != null)
            {
                listItems.Add("Sunday workout");
            }
            if (trainingProgram.MondayWorkoutId != null)
            {
                listItems.Add("Monday workout");
            }
            if (trainingProgram.TuesdayWorkoutId != null)
            {
                listItems.Add("Tuesday workout");
            }

            if (trainingProgram.WednesdayWorkoutId != null)
            {
                listItems.Add("Wednesday workout");
            }
            if (trainingProgram.ThursdayWorkoutId != null)
            {
                listItems.Add("Thursday workout");
            }
            if (trainingProgram.FridayWorkoutId != null)
            {
                listItems.Add("Friday workout");
            }
            if (trainingProgram.SaturdayWorkoutId != null)
            {
                listItems.Add("Saturday workout");
            }

            workoutList.ItemsSource = listItems;
        }

        private void workoutList_ItemClick(object sender, ItemClickEventArgs e)
        {
            string item = (string)e.ClickedItem;
            string workoutId = trainingProgram.SundayWorkoutId;

            switch (item)
            {
                case "Sunday workout":
                    workoutId = trainingProgram.SundayWorkoutId;
                    break;
                case "Monday workout":
                    workoutId = trainingProgram.MondayWorkoutId;
                    break;
                case "Tuesday workout":
                    workoutId = trainingProgram.TuesdayWorkoutId;
                    break;
                case "Wednesday workout":
                    workoutId = trainingProgram.WednesdayWorkoutId;
                    break;
                case "Thursday workout":
                    workoutId = trainingProgram.ThursdayWorkoutId;
                    break;
                case "Friday workout":
                    workoutId = trainingProgram.FridayWorkoutId;
                    break;
                case "Saturday workout":
                    workoutId = trainingProgram.SaturdayWorkoutId;
                    break;
            }
            UserWorkoutStaticsParams.WorkoutId = workoutId;
            UserWorkoutStaticsParams.Myself = true;

            Frame.Navigate(typeof(UserWorkoutStatisticsDisplay), null);
        }


        async void Speak(string message)
        {
            var stream = await synth.SynthesizeTextToStreamAsync(message);
            media.SetSource(stream, stream.ContentType);
            media.Play();
        }
    }
}
