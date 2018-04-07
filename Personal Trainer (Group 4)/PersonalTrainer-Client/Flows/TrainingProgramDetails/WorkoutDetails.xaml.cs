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
    public sealed partial class WorkoutDetails : Page
    {
        private DefaultWorkout m_workout;

        private UserTrainingProgram m_trainingProgram;

        private List<DefaultExercise> m_exercises = new List<DefaultExercise>();

        private string m_workoutId;

        public WorkoutDetails()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;


            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = (WorkoutDetailsParams)e.Parameter;
            m_trainingProgram = parameters.trainingProgram;
            m_workout = parameters.workout;
            
            var tasks = new List<Task<DefaultExercise>>();


            var exerciseIds = m_workout.ExercisesIds.Split(';').ToList();

            ObservableCollection<string> listItems = new ObservableCollection<string>();


            foreach (var id in exerciseIds)
            {
                tasks.Add(App.DefaultExercisesTable.LookupAsync(id));

            }

            var results = (await Task.WhenAll(tasks)).ToList();

            foreach(var result in results)
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

            Frame.Navigate(typeof(ExerciseDetails), new ExerciseDetailsParams(ex, m_trainingProgram));

        }
    }
}
