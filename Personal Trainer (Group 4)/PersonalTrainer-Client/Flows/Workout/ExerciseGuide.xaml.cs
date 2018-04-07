using PersonalTrainer_Client.DataObjects;
using PersonalTrainer_Client.DataObjects.navigationsParameters;
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

namespace PersonalTrainer_Client.Flows.Workout
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExerciseGuide : Page
    {
        private DefaultExercise m_exercise { get; set; }
        private ExerciseParams parameters { get; set; }

        private BitmapImage bitmapImage1;
        private BitmapImage bitmapImage2;
        private bool isFirstDisplayed = true;

        public ExerciseGuide()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            m_exercise = (DefaultExercise)e.Parameter;

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

            String[] rows = m_exercise.HowToPerform.Split('.').Select(row => row.Trim()).ToArray();
            GuideText.Text = String.Join(".\n", rows);
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
