using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PersonalTrainer_Client.DataObjects;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PersonalTrainer_Client.Flows.Registration
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserDeatailsPage : Page
    {
        int currentForm = 0;
        string userId;

        public UserDeatailsPage()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App.App_BackRequested;

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                userId = e.Parameter.ToString();
            }
           
            base.OnNavigatedTo(e);
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            currentForm--;
            ShowCurrentForm();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            currentForm++;
            if (currentForm == 4)
            {
                User user = CreateUser();
                Frame.Navigate(typeof(ChooseTrainingProgram), user);
            }
            else
            {
                ShowCurrentForm();
            }
        }

        private void ShowCurrentForm()
        {
            Form_1.Visibility = Visibility.Collapsed;
            Form_2.Visibility = Visibility.Collapsed;
            Form_3.Visibility = Visibility.Collapsed;
            Form_4.Visibility = Visibility.Collapsed;
            if (currentForm == 0)
            {
                Form_1.Visibility = Visibility.Visible;

                BtnPrevious.IsEnabled = false;
                BtnNext.IsEnabled = true;
                BtnNext.Content = "Next";

            }
            else if (currentForm == 1)
            {
                Form_2.Visibility = Visibility.Visible;

                BtnPrevious.IsEnabled = true;
                BtnNext.IsEnabled = true;
                BtnNext.Content = "Next";

            }
            else if (currentForm == 2)
            {
                Form_3.Visibility = Visibility.Visible;

                BtnPrevious.IsEnabled = true;
                BtnNext.IsEnabled = true;
                BtnNext.Content = "Next";

            }
            else if (currentForm == 3)
            {
                Form_4.Visibility = Visibility.Visible;

                BtnPrevious.IsEnabled = true;
                BtnNext.IsEnabled = CanFinish();
                BtnNext.Content = "Finish";
         
            }
        }

        private void CanFinish(object sender, RoutedEventArgs e)
        {
            BtnNext.IsEnabled = CanFinish();
        }

        private bool CanFinish()
        {
            int i = 0;
            if (String.IsNullOrEmpty(TxtUserName.Text) || String.IsNullOrEmpty(TxtAge.Text) ||
                String.IsNullOrEmpty(TxtHeight.Text) || String.IsNullOrEmpty(TxtWeight.Text) ||
                String.IsNullOrEmpty(TxtExerciseAverage.Text)  || !int.TryParse(TxtAge.Text, out i) ||
                !int.TryParse(TxtHeight.Text, out i) || !int.TryParse(TxtWeight.Text, out i) ||
                !int.TryParse(TxtExerciseAverage.Text, out i) || (!(bool)DaySunday.IsChecked && 
                !(bool)DayMonday.IsChecked && !(bool)DayTuesday.IsChecked &&
                !(bool)DayWednesday.IsChecked && !(bool)DayThursday.IsChecked && 
                !(bool)DayFriday.IsChecked && !(bool)DaySaturday.IsChecked ))
            {
                return false;
            }
            return true;
        }

        private User CreateUser()
        {
            User user = new User
            {
                Name = TxtUserName.Text,
                Age = TxtAge.Text,
                Gender = (bool)GenMale.IsChecked ? Gender.Male : Gender.Female,
                Height = TxtHeight.Text,
                Weight = TxtWeight.Text
            };

            if ((bool)GoalStrength.IsChecked)
            {
                user.PreferredTrainingGoal = TrainingProgramGoal.Strength;
            }
            else if ((bool)GoalSize.IsChecked)
            {
                user.PreferredTrainingGoal = TrainingProgramGoal.Size;
            }
            else if ((bool)GoalFitness.IsChecked)
            {
                user.PreferredTrainingGoal = TrainingProgramGoal.Fitness;
            }
            else if ((bool)GoalWeightLoss.IsChecked)
            {
                user.PreferredTrainingGoal = TrainingProgramGoal.WeightLoss;
            }
            else if ((bool)GoalSpeed.IsChecked)
            {
                user.PreferredTrainingGoal = TrainingProgramGoal.Speed;
            }
            else if ((bool)GoalFlexibility.IsChecked)
            {
                user.PreferredTrainingGoal = TrainingProgramGoal.Flexibility;
            }

            bool mustHaveGym = user.PreferredTrainingGoal == TrainingProgramGoal.Strength || user.PreferredTrainingGoal == TrainingProgramGoal.Size
                || user.PreferredTrainingGoal == TrainingProgramGoal.Fitness;
            bool hasGymAccess = (bool)GymAccess.IsChecked;
            if (mustHaveGym && hasGymAccess)
            {
                user.PreferredTrainingProgramType = TrainingProgramType.Gym;
            }
            else if (!mustHaveGym && hasGymAccess)
            {
                user.PreferredTrainingProgramType = TrainingProgramType.GymAndOutside;
            }
            else
            {
                user.PreferredTrainingProgramType = TrainingProgramType.Outside;
            }

            int workHours = Int32.Parse(TxtExerciseAverage.Text);
            if (workHours >= 0 && workHours <= 3)
            {
                user.PreferredTrainingProgramDifficulty = Difficulty.Begginer;
            }
            else if (workHours > 3 && workHours <= 6)
            {
                user.PreferredTrainingProgramDifficulty = Difficulty.Intermediate;
            }
            else
            {
                user.PreferredTrainingProgramDifficulty = Difficulty.Expert;
            }

            string PreferredWorkoutDays = "";
            if ((bool)DaySunday.IsChecked)
            {
                PreferredWorkoutDays += "1;";
            }
            if ((bool)DayMonday.IsChecked)
            {
                PreferredWorkoutDays += "2;";
            }
            if ((bool)DayTuesday.IsChecked)
            {
                PreferredWorkoutDays += "3;";
            }
            if ((bool)DayWednesday.IsChecked)
            {
                PreferredWorkoutDays += "4;";
            }
            if ((bool)DayThursday.IsChecked)
            {
                PreferredWorkoutDays += "5;";
            }
            if ((bool)DayFriday.IsChecked)
            {
                PreferredWorkoutDays += "6;";
            }
            if ((bool)DaySaturday.IsChecked)
            {
                PreferredWorkoutDays += "7;";
            }
            PreferredWorkoutDays = PreferredWorkoutDays.Substring(0, PreferredWorkoutDays.Length - 1);
            user.PreferredWorkoutDays = PreferredWorkoutDays;

            user.AuthenticationKey = userId;
            user.Id = userId;


            return user;
        }
    }
}
