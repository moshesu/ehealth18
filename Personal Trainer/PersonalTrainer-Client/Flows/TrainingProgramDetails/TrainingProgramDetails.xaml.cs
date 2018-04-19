using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace PersonalTrainer_Client.Flows.TrainingProgramDetails
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TrainingProgramDetails : Page
    {
        private UserTrainingProgram trainingProgram;

        private DefaultWorkout m_workout;

        private List<DefaultExercise> m_exercises = new List<DefaultExercise>();

        private string m_workoutId;

        public TrainingProgramDetails()
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

        private async void workoutList_ItemClickAsync(object sender, ItemClickEventArgs e)
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

            var parameters = new WorkoutDetailsParams(workoutId, trainingProgram);
            
            var tasks = new List<Task<DefaultExercise>>();

            m_workout = parameters.workout;

            if (parameters.workout == null)
            {
                m_workoutId = parameters.workoutId;
                m_workout = await App.DefaultWorkoutsTable.LookupAsync(m_workoutId);
            }

            trainingProgram = parameters.trainingProgram;

            var exerciseIds = m_workout.ExercisesIds.Split(';').ToList();

            ObservableCollection<string> listItems = new ObservableCollection<string>();


            foreach (var id in exerciseIds)
            {
                tasks.Add(App.DefaultExercisesTable.LookupAsync(id));

            }

            var results = (await Task.WhenAll(tasks)).ToList();

            foreach (var result in results)
            {
                var ex = await App.DefaultExercisesTable.LookupAsync(result.Id);
                m_exercises.Add(ex);

                listItems.Add(ex.Name);
            }

            exercisesList.ItemsSource = listItems;

        }

        private void exercisesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            DefaultExercise ex = m_exercises.Find(exe => exe.Name.Equals((string)e.ClickedItem));

            Frame.Navigate(typeof(ExerciseDetails), new ExerciseDetailsParams(ex, trainingProgram));

        }
    }
}
