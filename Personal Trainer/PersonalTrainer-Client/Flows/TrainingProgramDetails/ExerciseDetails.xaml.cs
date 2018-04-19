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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PersonalTrainer_Client.Flows.TrainingProgramDetails
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    


    public sealed partial class ExerciseDetails : Page
    {
        private DefaultExercise m_exercise;
        private UserTrainingProgram m_trainingProgram;


        private BitmapImage bitmapImage1;
        private BitmapImage bitmapImage2;
        private bool isFirstDisplayed = true;

        public ExerciseDetails()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;


            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = (ExerciseDetailsParams)e.Parameter;

            m_exercise = parameters.ex;
            m_trainingProgram = parameters.trainingProgram;

            bitmapImage1 = new BitmapImage();
            bitmapImage2 = new BitmapImage();
            GuidImage.Width = bitmapImage1.DecodePixelWidth = 800; //natural px width of image source
                                                                   // don't need to set Height, system maintains aspect ratio, and calculates the other
                                                                   // dimension, so long as one dimension measurement is provided
            GuidImage.Width = bitmapImage2.DecodePixelWidth = 800;
            bitmapImage1.UriSource = new Uri(m_exercise.GuidnaceImages.Split(';')[0]);
            bitmapImage2.UriSource = new Uri(m_exercise.GuidnaceImages.Split(';')[1]);
            //set image source
            GuidImage.Source = bitmapImage1;

            // Time
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            Name.Text = m_exercise.Name;
            Muscle.Text = m_exercise.getMuscleName();
            switch(m_trainingProgram.Difficulty)
            {
                case Difficulty.Begginer:
                    Reps.Text = m_exercise.BegginerReps.ToString();
                    Sets.Text = m_exercise.BegginerSets.ToString();
                    break;
                case Difficulty.Intermediate:
                    Reps.Text = m_exercise.IntermediateReps.ToString();
                    Sets.Text = m_exercise.IntermediateSets.ToString();
                    break;
                case Difficulty.Expert:
                    Reps.Text = m_exercise.ExpertReps.ToString();
                    Sets.Text = m_exercise.ExpertSets.ToString();
                    break;
            }                
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            if (isFirstDisplayed)
            {
                GuidImage.Source = bitmapImage2;
            }
            else
            {
                GuidImage.Source = bitmapImage1;
            }
            isFirstDisplayed = !isFirstDisplayed;
        }
    }

    
}
