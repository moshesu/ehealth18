using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
using PersonalTrainer_Client.Flows.MainFlow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PersonalTrainer_Client.Flows.Workout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Exercise : Page
    {
        private DefaultExercise m_currentExercise;
        private int m_currentExresiceIndex;
        private int m_numberOfSets;
        private int m_weight;
        private int m_numberOfSetsRemainedToPerform;

        CaloriesModel _caloriesModel = new CaloriesModel();

        public Exercise() 
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var exerciseId = ExerciseParams.exerciseId;

            // Timer
            App.workoutTimer.Tick += dispatcherTimer_Tick;

            if(m_numberOfSets == 0 || ExerciseParams.startNewExercise)
            {
                m_currentExercise = await App.DefaultExercisesTable.LookupAsync(exerciseId);

                switch (MainDashboard.CurrentUser.PreferredTrainingProgramDifficulty)
                {
                    case Difficulty.Begginer:
                        m_numberOfSets = m_currentExercise.BegginerSets;
                        m_weight = m_currentExercise.BegginerWeight;
                        break;
                    case Difficulty.Intermediate:
                        m_numberOfSets = m_currentExercise.IntermediateSets;
                        m_weight = m_currentExercise.IntermediateWeight;
                        break;
                    case Difficulty.Expert:
                        m_numberOfSets = m_currentExercise.ExpertSets;
                        m_weight = m_currentExercise.ExpertWeight;
                        break;
                    default:
                        m_numberOfSets = m_currentExercise.BegginerSets;
                        m_weight = m_currentExercise.BegginerWeight;
                        break;
                }
                m_numberOfSetsRemainedToPerform = m_numberOfSets;
                ExerciseParams.startNewExercise = false;
            }

            if (m_currentExercise == null)
            {
                m_currentExercise = await App.DefaultExercisesTable.LookupAsync(exerciseId);
            }

            if (ExerciseParams.isSetFinished)
            {
                m_numberOfSetsRemainedToPerform--;
                ExerciseParams.isSetFinished = false;
            }

            if(m_numberOfSetsRemainedToPerform <= 0)
            {
                StartSet.IsEnabled = false;
                StartSet.Visibility = Visibility.Collapsed;
                RemainSets.Text = "Done";
                RemainSets.FontSize = 30;
                WorkoutParams.isFinishedExercise = true;
            }
            else
            {
                StartSet.IsEnabled = true;
                StartSet.Visibility = Visibility.Visible;
                RemainSets.Text = m_numberOfSetsRemainedToPerform.ToString() + " Sets remain * " + m_weight + " Kg";
                RemainSets.FontSize = 20;
            }

            ExerciseName.Text = m_currentExercise.Name;          

            // Calories
            
            _caloriesModel.Init();
            _caloriesModel.Changed += _caloriesModel_Changed;
            _caloriesModel.Start();

            ShowGuid.IsEnabled = true;
            FinishExercise.IsEnabled = true;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _caloriesModel.Changed -= _caloriesModel_Changed;
            _caloriesModel.Stop();
        }

        private void StartSet_Click(object sender, RoutedEventArgs e)
        {
            SetParams.exercise = m_currentExercise;
            Frame.Navigate(typeof(Set), null);
        }

        private void ShowGuid_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExerciseGuide), m_currentExercise);
        }

        private void FinishExercise_Click(object sender, RoutedEventArgs e)
        {
            WorkoutParams.isFinishedExercise = true;

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

        private void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            TimeSpan span = now - App.startTime;
            time.Text = span.ToString(@"mm\:ss");
        }

        void _caloriesModel_Changed(long cal)
        {
            calories.Text = "" + cal;
        }
    }
}
